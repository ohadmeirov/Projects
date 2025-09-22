package com.hit.model;

import com.google.gson.annotations.SerializedName;
import java.time.LocalTime;

public class ReservationRequest {
    private Customer customer;
    private Restaurant restaurant;
    
    @SerializedName("timeSlot")
    private String timeSlotStr;
    
    @SerializedName("dayOfWeek")
    private String dayOfWeekStr;
    
    private StrategyType strategyType;

    public ReservationRequest() {
    }

    public ReservationRequest(Customer customer, Restaurant restaurant, String timeSlotStr,
                            String dayOfWeekStr, StrategyType strategyType) {
        this.customer = customer;
        this.restaurant = restaurant;
        this.timeSlotStr = timeSlotStr;
        this.dayOfWeekStr = dayOfWeekStr;
        this.strategyType = strategyType;
    }

    public Customer getCustomer() {
        return customer;
    }

    public void setCustomer(Customer customer) {
        this.customer = customer;
    }

    public Restaurant getRestaurant() {
        return restaurant;
    }

    public void setRestaurant(Restaurant restaurant) {
        this.restaurant = restaurant;
    }

    public TimeSlot getTimeSlot() {
        if (timeSlotStr != null) {
            return new TimeSlot(getDayOfWeek(), LocalTime.parse(timeSlotStr));
        }
        return null;
    }

    public DayOfTheWeek getDayOfWeek() {
        if (dayOfWeekStr != null) {
            return DayOfTheWeek.valueOf(dayOfWeekStr);
        }
        return null;
    }

    public StrategyType getStrategyType() {
        return strategyType;
    }

    public void setStrategyType(StrategyType strategyType) {
        this.strategyType = strategyType;
    }
}