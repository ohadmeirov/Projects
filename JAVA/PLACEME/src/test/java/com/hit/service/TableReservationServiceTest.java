package com.hit.service;

import com.hit.model.StrategyType;
import com.hit.dao.IDao;
import com.hit.model.*;
import com.hit.algorithm.TableReservationContext;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.mockito.Spy;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class TableReservationServiceTest {
    @Mock
    private IDao mockDao;
    @Mock
    private TableReservationContext mockReservationContext;
    private List<Table> tables;

    private TableReservationServiceImpl service;
    private Customer testCustomer;
    private Restaurant testRestaurant;
    private TimeSlot testTimeSlot;
    private DayOfTheWeek testDay;
    private Table testTable;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
        service = new TableReservationServiceImpl(mockDao, mockReservationContext);
        tables = new ArrayList<>();
        
        // Initialize test data
        testCustomer = new Customer("Test Customer", 2, DayOfTheWeek.MONDAY, LocalTime.of(13, 0));
        testRestaurant = new Restaurant("Test Restaurant", tables, 8, 22);
        testTimeSlot = new TimeSlot(DayOfTheWeek.MONDAY, LocalTime.of(13, 0));
        testDay = DayOfTheWeek.MONDAY;
        testTable = new Table(1, 4);
    }

    @Test
    void reserveTable_Success() {
        // Arrange
        when(mockDao.saveReservation(any(), any(), any(), any(), any())).thenReturn(true);
        when(mockReservationContext.reserveTable(any())).thenReturn(testTable);
        
        // Act
        Table result = service.reserveTable(testCustomer, testRestaurant, testTimeSlot, 
                                          testDay, StrategyType.OPTIMIZED);
        
        // Assert
        assertNotNull(result);
        verify(mockDao).saveReservation(testCustomer, testRestaurant, result, testTimeSlot, testDay);
    }

    @Test
    void cancelReservation_Success() {
        // Arrange
        when(mockDao.deleteReservation(any(), any(), any(), any())).thenReturn(true);
        when(mockReservationContext.cancelReservation(any())).thenReturn(true);
        
        // Act
        boolean result = service.cancelReservation(testCustomer, testRestaurant, testTimeSlot, testDay);
        
        // Assert
        assertTrue(result);
        verify(mockDao).deleteReservation(testCustomer, testRestaurant, testTimeSlot, testDay);
    }

    @Test
    void getAvailableTables_Success() {
        // Act
        Set<Table> availableTables = service.getAvailableTables(testRestaurant, testTimeSlot, testDay);
        
        // Assert
        assertNotNull(availableTables);
    }

    @Test
    void getCustomerReservations_Success() {
        // Arrange
        List<TimeSlot> expectedSlots = List.of(testTimeSlot);
        when(mockDao.getCustomerReservations(testCustomer)).thenReturn(expectedSlots);
        
        // Act
        List<TimeSlot> result = service.getCustomerReservations(testCustomer);
        
        // Assert
        assertNotNull(result);
        assertEquals(expectedSlots, result);
        verify(mockDao).getCustomerReservations(testCustomer);
    }

    @Test
    void getRestaurantReservations_Success() {
        // Arrange
        List<TimeSlot> expectedSlots = List.of(testTimeSlot);
        when(mockDao.getRestaurantReservations(testRestaurant, testDay)).thenReturn(expectedSlots);
        
        // Act
        List<TimeSlot> result = service.getRestaurantReservations(testRestaurant, testDay);
        
        // Assert
        assertNotNull(result);
        assertEquals(expectedSlots, result);
        verify(mockDao).getRestaurantReservations(testRestaurant, testDay);
    }

    @Test
    void reserveTable_NullInputs() {
        // Test null customer
        assertThrows(NullPointerException.class, () -> 
            service.reserveTable(null, testRestaurant, testTimeSlot, testDay, StrategyType.OPTIMIZED));

        // Test null restaurant
        assertThrows(NullPointerException.class, () -> 
            service.reserveTable(testCustomer, null, testTimeSlot, testDay, StrategyType.OPTIMIZED));

        // Test null time slot
        assertThrows(NullPointerException.class, () -> 
            service.reserveTable(testCustomer, testRestaurant, null, testDay, StrategyType.OPTIMIZED));

        // Test null day
        assertThrows(NullPointerException.class, () -> 
            service.reserveTable(testCustomer, testRestaurant, testTimeSlot, null, StrategyType.OPTIMIZED));

        // Test null strategy
        assertThrows(NullPointerException.class, () -> 
            service.reserveTable(testCustomer, testRestaurant, testTimeSlot, testDay, null));
    }

    @Test
    void cancelReservation_NullInputs() {
        // Test null customer
        assertThrows(NullPointerException.class, () -> 
            service.cancelReservation(null, testRestaurant, testTimeSlot, testDay));

        // Test null restaurant
        assertThrows(NullPointerException.class, () -> 
            service.cancelReservation(testCustomer, null, testTimeSlot, testDay));

        // Test null time slot
        assertThrows(NullPointerException.class, () -> 
            service.cancelReservation(testCustomer, testRestaurant, null, testDay));

        // Test null day
        assertThrows(NullPointerException.class, () -> 
            service.cancelReservation(testCustomer, testRestaurant, testTimeSlot, null));
    }
} 