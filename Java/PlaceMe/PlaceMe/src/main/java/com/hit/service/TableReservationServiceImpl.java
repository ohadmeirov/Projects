package com.hit.service;

import com.hit.algorithm.IAlgoTableReservationStrategy;
import com.hit.algorithm.ReservationStrategyFactory;
import com.hit.algorithm.TableReservationContext;
import com.hit.dao.IDao;
import com.hit.model.*;

import java.util.List;
import java.util.Objects;
import java.util.Set;

public class TableReservationServiceImpl implements ITableReservationService {
    private final IDao dao;
    private final TableReservationContext reservationContext;
    private final RestaurantService restaurantService;

    public TableReservationServiceImpl(IDao dao, StrategyType strategyType) {
        this.dao = Objects.requireNonNull(dao, "DAO cannot be null");
        this.restaurantService = new RestaurantService();
        Restaurant defaultRestaurant = restaurantService.getDefaultRestaurant();
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(strategyType);
        this.reservationContext = new TableReservationContext(defaultRestaurant, strategy);
    }

    // For testing
    protected TableReservationServiceImpl(IDao dao, TableReservationContext reservationContext) {
        this.dao = Objects.requireNonNull(dao, "DAO cannot be null");
        this.restaurantService = new RestaurantService();
        this.reservationContext = Objects.requireNonNull(reservationContext, "ReservationContext cannot be null");
    }

    @Override
    public Table reserveTable(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                              DayOfTheWeek dayOfWeek, StrategyType strategyType) {
        // Validate inputs
        validateInputs(customer, restaurant, timeSlot, dayOfWeek, strategyType);

        // Get the real restaurant from DB/memory
        Restaurant realRestaurant = restaurantService.getRestaurantByName(restaurant.getName());
        if (realRestaurant == null) {
            return null;
        }

        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(strategyType);
        TableReservationContext reservationContext = new TableReservationContext(realRestaurant, strategy);

        Table table = reservationContext.reserveTable(customer);
        if (table != null) {
            dao.saveReservation(customer, realRestaurant, table, timeSlot, dayOfWeek);
            // Print reservations after save
            System.out.println("Customer reservations AFTER SAVE: " + dao.getCustomerReservations(customer));
            System.out.println("Restaurant reservations AFTER SAVE: " + dao.getRestaurantReservations(realRestaurant, dayOfWeek));
        }
        return table;
    }

    @Override
    public boolean cancelReservation(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                     DayOfTheWeek dayOfWeek) {
        // Validate inputs
        validateInputs(customer, restaurant, timeSlot, dayOfWeek);

        Restaurant realRestaurant = restaurantService.getRestaurantByName(restaurant.getName());
        if (realRestaurant == null) {
            return false;
        }

        List<TimeSlot> customerReservations = dao.getCustomerReservations(customer);
        List<TimeSlot> restaurantReservations = dao.getRestaurantReservations(realRestaurant, dayOfWeek);
        System.out.println("Customer reservations: " + customerReservations);
        System.out.println("Restaurant reservations: " + restaurantReservations);

        // Debug: print parameters sent to DAO
        System.out.println("Trying to delete reservation with params:");
        System.out.println("customer=" + customer.getName()
            + ", restaurant=" + realRestaurant.getName()
            + ", day=" + dayOfWeek
            + ", time=" + timeSlot.getTime());

        boolean deleted = dao.deleteReservation(customer, realRestaurant, timeSlot, dayOfWeek);
        System.out.println("Rows affected in deleteReservation: " + (deleted ? 1 : 0));
        if (!deleted) {
            System.out.println("Failed to delete reservation from DB. "
                + "Check that all parameters (customer, restaurant, day, time) match exactly what is stored in the DB.");
            return false;
        }

        // Call algorithm cancellation, but ignore its result for the response
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(StrategyType.OPTIMIZED);
        TableReservationContext reservationContext = new TableReservationContext(realRestaurant, strategy);
        reservationContext.cancelReservation(customer);

        System.out.println("Cancellation successful!");
        return true;
    }

    @Override
    public boolean joinToWaitlist(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                  DayOfTheWeek dayOfWeek) {
        // Validate inputs
        validateInputs(customer, restaurant, timeSlot, dayOfWeek);

        Restaurant realRestaurant = restaurantService.getRestaurantByName(restaurant.getName());
        if (realRestaurant == null) {
            return false;
        }

        // בדוק אם יש שולחן מתאים פנוי (כולל חפיפת טווחים)
        Table availableTable = realRestaurant.getTimeSlotManager()
            .findAvailableTable(realRestaurant, customer.getNumberOfGuests(), dayOfWeek, timeSlot.getTime());

        if (availableTable != null) {
            System.out.println("There is a suitable available table for this customer, cannot join waitlist.");
            return false;
        }

        // אם אין אף שולחן מתאים פנוי, אפשר להצטרף ל-WAITLIST (לא משנה כמה כבר יש בתור)
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(StrategyType.OPTIMIZED);
        TableReservationContext reservationContext = new TableReservationContext(realRestaurant, strategy);
        boolean result = reservationContext.joinToWaitlist(customer);
        if (result) {
            boolean dbResult = dao.addToWaitList(customer, realRestaurant, timeSlot, dayOfWeek);
            if (dbResult) {
                System.out.println("Successfully added to waitlist in DB.");
            } else {
                System.out.println("Failed to add to waitlist in DB.");
            }
            return dbResult;
        }
        return false;
    }

    public Set<Table> getAvailableTables(Restaurant restaurant, TimeSlot timeSlot, DayOfTheWeek dayOfWeek) {
        Restaurant realRestaurant = restaurantService.getRestaurantByName(restaurant.getName());
        if (realRestaurant == null) {
            return Set.of();
        }
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(StrategyType.OPTIMIZED);
        TableReservationContext reservationContext = new TableReservationContext(realRestaurant, strategy);
        return reservationContext.findAvailableTables(realRestaurant, dayOfWeek, timeSlot.getTime());
    }

    public List<TimeSlot> getCustomerReservations(Customer customer) {
        return dao.getCustomerReservations(customer);
    }

    public List<TimeSlot> getRestaurantReservations(Restaurant restaurant, DayOfTheWeek dayOfWeek) {
        return dao.getRestaurantReservations(restaurant, dayOfWeek);
    }

    private void validateInputs(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                DayOfTheWeek dayOfWeek) {
        Objects.requireNonNull(customer, "Customer cannot be null");
        Objects.requireNonNull(restaurant, "Restaurant cannot be null");
        Objects.requireNonNull(timeSlot, "Time slot cannot be null");
        Objects.requireNonNull(dayOfWeek, "Day of week cannot be null");
    }

    private void validateInputs(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                DayOfTheWeek dayOfWeek, StrategyType strategyType) {
        validateInputs(customer, restaurant, timeSlot, dayOfWeek);
        Objects.requireNonNull(strategyType, "Strategy type cannot be null");
    }
}