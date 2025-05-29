package com.hit.model;

public class Table {
    private int id;
    private int capacity;

    public void setId(int id) {
        this.id = id;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    private TableStatus status;

    public Table(int id, int capacity) {
        this.id = id;
        this.capacity = capacity;
        this.status = TableStatus.AVAILABLE;
    }

    public int getId() {
        return id;
    }

    public int getCapacity() {
        return capacity;
    }

    public TableStatus getStatus() {
        return status;
    }

    public void setStatus(TableStatus status) {
        this.status = status;
    }

    public void reserve() {
        this.status = TableStatus.RESERVED;
    }

    public void release() {
        this.status = TableStatus.AVAILABLE;
    }

    @Override
    public String toString() {
        return String.format("Table{id=%d, capacity=%d, status=%s}", id, capacity, status);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Table table = (Table) o;
        return id == table.id;
    }

    @Override
    public int hashCode() {
        return id;
    }
}


