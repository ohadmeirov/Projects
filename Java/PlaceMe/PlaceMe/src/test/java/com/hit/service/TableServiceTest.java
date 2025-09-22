package com.hit.service;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import com.hit.model.Table;
import com.hit.model.TableStatus;

public class TableServiceTest {
    private TableService tableService;
    private Table table;

    @BeforeEach
    public void setUp() {
        tableService = new TableService();
        table = tableService.getDefaultTable();
    }

    @Test
    public void testGetDefaultTable() {
        assertNotNull(table);
        assertEquals(1, tableService.getId(table));
        assertEquals(4, tableService.getCapacity(table));
        assertEquals(TableStatus.AVAILABLE, tableService.getStatus(table));
    }

    @Test
    public void testSetCapacity() {
        int newCapacity = 6;
        tableService.setCapacity(table, newCapacity);
        assertEquals(newCapacity, tableService.getCapacity(table));
    }

    @Test
    public void testSetStatus() {
        TableStatus newStatus = TableStatus.RESERVED;
        tableService.setStatus(table, newStatus);
        assertEquals(newStatus, tableService.getStatus(table));
    }
} 