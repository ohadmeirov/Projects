package com.hit.algorithm;

import com.hit.model.Restaurant;
import com.hit.model.Customer;
import com.hit.model.Table;
import com.hit.model.TimeSlot;
import com.hit.model.DayOfTheWeek;
import java.util.List;
import java.util.Set;
import java.time.LocalTime;
import java.util.HashSet;

// מחלקה זו מנהלת את האסטרטגיה הנוכחית להזמנת שולחנות במסעדה, ומאפשרת להחליף אסטרטגיה בזמן ריצה
public class TableReservationContext {
    private final Restaurant restaurant;
    private IAlgoTableReservationStrategy strategy;

    public TableReservationContext(Restaurant restaurant, IAlgoTableReservationStrategy strategy) {
        this.restaurant = restaurant;
        this.strategy = strategy;
    }

    public Table reserveTable(Customer customer) {
        return strategy.reserveTable(restaurant, customer);
    }

    public boolean cancelReservation(Customer customer) {
        return strategy.cancelReservation(restaurant, customer);
    }

    public boolean joinToWaitlist(Customer customer) {
        return strategy.joinToWaitlist(restaurant, customer);
    }

    public List<TimeSlot> findAlternativeTimeSlots(Customer customer) {
        return restaurant.getTimeSlotManager().findAlternativeTimeSlots(
            restaurant,
            customer.getNumberOfGuests(),
            customer.getDayToCome(),
            customer.getTimeToCome()
        );
    }

    public Table reserveTableAtTimeSlot(Customer customer, TimeSlot timeSlot) {
        return restaurant.getTimeSlotManager().reserveTable(
            restaurant,
            timeSlot.getTable(),
            customer,
            customer.getDayToCome(),
            customer.getTimeToCome()
        );
    }

    public Set<Table> findAvailableTables(Restaurant restaurant, DayOfTheWeek day, LocalTime time) {
        if (strategy instanceof OptimizedTableReservationStrategy) {
            return ((OptimizedTableReservationStrategy) strategy).getAvailableTables(restaurant, day, time);
        } else if (strategy instanceof NaiveTableReservationStrategy) {
            return ((NaiveTableReservationStrategy) strategy).getAvailableTables(restaurant, day, time);
        }
        return new HashSet<>();
    }

    public void setStrategy(IAlgoTableReservationStrategy strategy){
        this.strategy = strategy;
    }
} 