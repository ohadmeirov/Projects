package com.hit.model;

import java.util.*;

public class Restaurant {
    private String name;
    private List<Table> tables;
    private TimeSlotManager timeSlotManager;
    private int openingHour;
    private int closingHour;

    public void setName(String name) {
        this.name = name;
    }

    public void setTables(List<Table> tables) {
        this.tables = tables;
    }

    public void setTimeSlotManager(TimeSlotManager timeSlotManager) {
        this.timeSlotManager = timeSlotManager;
    }

    public void setOpeningHour(int openingHour) {
        this.openingHour = openingHour;
    }

    public void setClosingHour(int closingHour) {
        this.closingHour = closingHour;
    }

    public Restaurant(String name, List<Table> tables, int openingHour, int closingHour) {
        this.name = name;
        this.tables = new ArrayList<>(tables);
        this.openingHour = openingHour;
        this.closingHour = closingHour;
        this.timeSlotManager = new TimeSlotManager();
        this.timeSlotManager.initializeRestaurant(this);
    }

    public String getName() {
        return name;
    }

    public List<Table> getTables() {
        return tables;
    }

    public int getOpeningHour() {
        return openingHour;
    }

    public int getClosingHour() {
        return closingHour;
    }

    public TimeSlotManager getTimeSlotManager() {
        return timeSlotManager;
    }
}



