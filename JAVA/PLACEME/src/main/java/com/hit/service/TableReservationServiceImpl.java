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

        // Create and set strategy
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(strategyType);
        reservationContext.setStrategy(strategy);

        // Use strategy to find available table and reserve it
        Table table = reservationContext.reserveTable(customer);
        if (table != null) {
            dao.saveReservation(customer, restaurant, table, timeSlot, dayOfWeek);
        }
        return table;
    }

    @Override
    public boolean cancelReservation(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                   DayOfTheWeek dayOfWeek) {
        // Validate inputs
        validateInputs(customer, restaurant, timeSlot, dayOfWeek);

        // Cancel reservation using current strategy
        boolean result = reservationContext.cancelReservation(customer);
        if (result) {
            return dao.deleteReservation(customer, restaurant, timeSlot, dayOfWeek);
        }
        return false;
    }

    @Override
    public boolean joinToWaitlist(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                               DayOfTheWeek dayOfWeek) {
        // Validate inputs
        validateInputs(customer, restaurant, timeSlot, dayOfWeek);

        // Add to wait list using current strategy
        boolean result = reservationContext.joinToWaitlist(customer);
        if (result) {
            return dao.addToWaitList(customer, restaurant, timeSlot, dayOfWeek);
        }
        return false;
    }

    public Set<Table> getAvailableTables(Restaurant restaurant, TimeSlot timeSlot, DayOfTheWeek dayOfWeek) {
        // Use the default strategy (Optimized) to find available tables
        IAlgoTableReservationStrategy strategy = ReservationStrategyFactory.createStrategy(StrategyType.OPTIMIZED);
        reservationContext.setStrategy(strategy);
        return reservationContext.findAvailableTables(restaurant, dayOfWeek, timeSlot.getTime());
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