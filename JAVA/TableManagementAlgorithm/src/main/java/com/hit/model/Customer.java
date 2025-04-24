package com.hit.model;

/**
 * מייצג לקוח שמזמין מקום במסעדה.
 */

import java.time.LocalTime;

public class Customer {
    private final String name;
    private final int numberOfGuests;
    private final LocalTime timeToCome;
    private final DayOfTheWeek dayToCome;

    private Customer(Builder builder) {
        this.name = builder.name;
        this.numberOfGuests = builder.numberOfGuests;
        this.timeToCome = builder.timeToCome;
        this.dayToCome = builder.dayToCome;
    }

    public String getName() {
        return name;
    }

    public int getNumberOfGuests() {
        return numberOfGuests;
    }

    public LocalTime getTimeToCome() {
        return timeToCome;
    }

    public DayOfTheWeek getDayToCome() {
        return dayToCome;
    }

    public LocalTime getReservationTime() {
        return timeToCome;
    }

    @Override
    public String toString() {
        return "Customer{name='" + name + "', guests=" + numberOfGuests +
                ", time=" + timeToCome + ", day=" + dayToCome + "}";
    }

    public static class Builder {
        private String name;
        private int numberOfGuests;
        private LocalTime timeToCome;
        private DayOfTheWeek dayToCome;

        public Builder setName(String name) {
            this.name = name;
            return this;
        }

        public Builder setNumberOfGuests(int numberOfGuests) {
            this.numberOfGuests = numberOfGuests;
            return this;
        }

        public Builder setTimeToCome(String time) {
            try {
                LocalTime parsedTime = LocalTime.parse(time);

                int hour = parsedTime.getHour();
                int minute = parsedTime.getMinute();

                if (hour < 9 || hour > 23 || (hour == 23 && minute > 0)) {
                    throw new IllegalArgumentException("Time must be between 09:00 and 23:00.");
                }

                if (minute != 0 && minute != 30) {
                    throw new IllegalArgumentException("Only full or half hours are allowed (e.g., 09:00, 09:30).");
                }

                this.timeToCome = parsedTime;
            } catch (Exception e) {
                throw new IllegalArgumentException("Invalid time format. Use HH:mm, e.g., 09:00.");
            }

            return this;
        }

        public Builder setDayToCome(DayOfTheWeek day) {
            this.dayToCome = day;
            return this;
        }

        public Customer build() {
            validate();
            return new Customer(this);
        }

        private void validate() {
            if (name == null || name.isBlank()) {
                throw new IllegalArgumentException("Customer name must not be null or empty.");
            }
            if (numberOfGuests <= 0) {
                throw new IllegalArgumentException("Number of guests must be greater than 0.");
            }
            if (timeToCome == null) {
                throw new IllegalArgumentException("Time must be provided.");
            }
            if (dayToCome == null) {
                throw new IllegalArgumentException("Day must be provided.");
            }
        }
    }
}

