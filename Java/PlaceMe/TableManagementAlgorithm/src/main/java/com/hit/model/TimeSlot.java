package com.hit.model;

import java.time.LocalTime;
import java.util.Objects;

public class TimeSlot {
    private Table table;
    private final DayOfTheWeek day;
    private final LocalTime time;

    public TimeSlot(Table table, DayOfTheWeek day, LocalTime time) {
        this.table = table;
        this.day = day;
        this.time = time;
    }

    public TimeSlot(DayOfTheWeek day, LocalTime time) {
        this.day = day;
        this.time = time;
    }

    public Table getTable() {
        return table;
    }

    public void setTable(Table table) {
        this.table = table;
    }

    public DayOfTheWeek getDay() {
        return day;
    }

    public LocalTime getTime() {
        return time;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        TimeSlot timeSlot = (TimeSlot) o;
        return day == timeSlot.day && Objects.equals(time, timeSlot.time);
    }

    @Override
    public int hashCode() {
        return Objects.hash(day, time);
    }

    @Override
    public String toString() {
        return day + " at " + time;
    }
} 