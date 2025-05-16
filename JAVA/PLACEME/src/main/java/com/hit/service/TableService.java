package com.hit.service;

import com.hit.model.Table;
import com.hit.model.TableStatus;

public class TableService {
    // Business logic for tables
    ///•	כל Service מקבל Dao ו־Algorithm (Strategy) ב־constructor
    /// •	Service מבצע את כל הלוגיקה העסקית.

    public Table getDefaultTable() {
        // For now, return a stub Table with default values
        return new Table(1, 4);
    }

    // SET operations
    public void setCapacity(Table table, int newCapacity) {
        table.setCapacity(newCapacity);
    }

    public void setStatus(Table table, TableStatus newStatus) {
        table.setStatus(newStatus);
    }

    // GET operations
    public int getCapacity(Table table) {
        return table.getCapacity();
    }

    public TableStatus getStatus(Table table) {
        return table.getStatus();
    }

    public int getId(Table table) {
        return table.getId();
    }
} 