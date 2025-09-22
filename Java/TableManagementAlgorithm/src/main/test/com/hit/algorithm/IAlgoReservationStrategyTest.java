package com.hit.algorithm;

import com.hit.model.Customer;
import com.hit.model.Restaurant;
import com.hit.model.Table;
import com.hit.model.DayOfTheWeek;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

public class IAlgoReservationStrategyTest {
    private Restaurant restaurant;
    private Customer customer;

    @BeforeEach
    void setUp() {
        // יוצר רשימת שולחנות
        List<Table> tables = new ArrayList<>();
        tables.add(new Table(1, 2));  // שולחן ל-2
        tables.add(new Table(2, 4));  // שולחן ל-4
        tables.add(new Table(3, 6));  // שולחן ל-6
        tables.add(new Table(4, 8));  // שולחן ל-8

        // יוצר מסעדה עם השולחנות
        restaurant = new Restaurant("Test Restaurant", tables, 10, 22);

        // יוצר לקוח - עכשיו משתמשים ב-com.hit.model.DayOfWeek במקום java.time.DayOfWeek
        customer = new Customer("Test Customer", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
    }

    @Nested
    class OptimizedStrategyTests {
        private TableReservationContext context;

        @BeforeEach
        void setUp() {
            context = new TableReservationContext(restaurant, new OptimizedTableReservationStrategy());
        }

        @Test
        void testExactFitReservation() {
            // מנסה להזמין שולחן ל-2 אנשים
            Table reservedTable = context.reserveTable(customer);
            assertNotNull(reservedTable);
            assertEquals(1, reservedTable.getId()); // שולחן 1 הוא בדיוק בגודל 2
        }

        @Test
        void testPlusOneReservation() {
            // מזמין את השולחן של 2
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);

            // מנסה להזמין שולחן ל-2 אנשים נוספים - שימו לב שזה לקוח חדש
            Customer customer3 = new Customer("Test Customer 3", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(customer3);
            assertNotNull(reservedTable);
            assertEquals(2, reservedTable.getId()); // שולחן 2 הוא בגודל 4
        }

        @Test
        void testPlusTwoReservation() {
            // מזמין את השולחנים של 2 ו-4
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);

            // מנסה להזמין שולחן ל-2 אנשים נוספים - שימו לב שזה לקוח חדש
            Customer customer4 = new Customer("Test Customer 4", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(customer4);
            assertNotNull(reservedTable);
            assertEquals(3, reservedTable.getId()); // שולחן 3 הוא בגודל 6
        }

        @Test
        void testNoSuitableTable() {
            // מזמין את כל השולחנות
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer4 = new Customer("Customer 4", 6, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer5 = new Customer("Customer 5", 8, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);
            context.reserveTable(customer4);
            context.reserveTable(customer5);

            // מנסה להזמין שולחן ל-2 אנשים נוספים - שימו לב שזה לקוח חדש
            Customer customer6 = new Customer("Test Customer 6", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(customer6);
            assertNull(reservedTable);
        }

        @Test
        void testCancelReservation() {
            // מזמין שולחן
            Table reservedTable = context.reserveTable(customer);
            assertNotNull(reservedTable);

            // מבטל את ההזמנה
            boolean cancelled = context.cancelReservation(customer);
            assertTrue(cancelled);

            // בודק שאפשר להזמין את השולחן שוב
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable2 = context.reserveTable(customer2);
            assertNotNull(reservedTable2);
            assertEquals(reservedTable.getId(), reservedTable2.getId());
        }

        @Test
        void testJoinToWaitlist() {
            // מזמין את כל השולחנות
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer4 = new Customer("Customer 4", 6, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer5 = new Customer("Customer 5", 8, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);
            context.reserveTable(customer4);
            context.reserveTable(customer5);

            // מנסה להצטרף לתור המתנה
            boolean joined = context.joinToWaitlist(customer);
            assertTrue(joined);
        }
    }

    @Nested
    class NaiveStrategyTests {
        private TableReservationContext context;

        @BeforeEach
        void setUp() {
            context = new TableReservationContext(restaurant, new NaiveTableReservationStrategy());
        }

        @Test
        void testFirstAvailableTableReservation() {
            // מנסה להזמין שולחן ל-2 אנשים
            Table reservedTable = context.reserveTable(customer);
            assertNotNull(reservedTable);
            assertTrue(reservedTable.getCapacity() >= customer.getNumberOfGuests());
        }

        @Test
        void testLargeTableReservation() {
            // מזמין את השולחן הקטן
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);

            // מנסה להזמין שולחן ל-2 אנשים נוספים
            Customer customer3 = new Customer("Customer 3", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(customer3);
            assertNotNull(reservedTable);
            assertTrue(reservedTable.getCapacity() >= customer3.getNumberOfGuests());
            // בנאיבי, זה יכול להיות כל שולחן פנוי שמתאים
        }

        @Test
        void testNoAvailableTable() {
            // מזמין את כל השולחנות
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer4 = new Customer("Customer 4", 6, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer5 = new Customer("Customer 5", 8, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);
            context.reserveTable(customer4);
            context.reserveTable(customer5);

            // מנסה להזמין שולחן ל-2 אנשים נוספים
            Customer customer6 = new Customer("Customer 6", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(customer6);
            assertNull(reservedTable);
        }

        @Test
        void testCancelReservation() {
            Table reservedTable = context.reserveTable(customer);
            assertNotNull(reservedTable);

            boolean cancelled = context.cancelReservation(customer);
            assertTrue(cancelled);

            // בודק שאפשר להזמין את השולחן שוב
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table reservedTable2 = context.reserveTable(customer2);
            assertNotNull(reservedTable2);
            // לא בודקים שזה אותו שולחן כי באסטרטגיה הנאיבית אין התחייבות לבחור שולחן מסוים
        }

        @Test
        void testJoinToWaitlist() {
            // מזמין את כל השולחנות
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer4 = new Customer("Customer 4", 6, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer5 = new Customer("Customer 5", 8, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);
            context.reserveTable(customer4);
            context.reserveTable(customer5);

            boolean joined = context.joinToWaitlist(customer);
            assertTrue(joined);
        }
    }

    @Nested
    class EdgeCasesTests {
        private TableReservationContext context;

        @BeforeEach
        void setUp() {
            context = new TableReservationContext(restaurant, new OptimizedTableReservationStrategy());
        }

        @Test
        void testReserveTableOutsideOpeningHours() {
            Customer lateCustomer = new Customer("Late Customer", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(23, 0));
            Table reservedTable = context.reserveTable(lateCustomer);
            assertNull(reservedTable);
        }

        @Test
        void testReserveTableWithTooManyGuests() {
            Customer largeGroup = new Customer("Large Group", 10, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(largeGroup);
            assertNull(reservedTable);
        }

        @Test
        void testReserveTableWithZeroGuests() {
            Customer invalidCustomer = new Customer("Invalid Customer", 0, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(invalidCustomer);
            assertNull(reservedTable);
        }

        @Test
        void testReserveTableWithNegativeGuests() {
            Customer invalidCustomer = new Customer("Invalid Customer", -1, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(invalidCustomer);
            assertNull(reservedTable);
        }

        @Test
        void testCancelNonExistentReservation() {
            // מנסה לבטל הזמנה שלא קיימת
            Customer nonExistingCustomer = new Customer("Non Existing Customer", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            boolean cancelled = context.cancelReservation(nonExistingCustomer);
            assertFalse(cancelled);
        }

        @Test
        void testReserveTableWithOverlappingTimes() {
            Table reservedTable = context.reserveTable(customer);
            assertNotNull(reservedTable);

            // בודק שלקוח אחר יכול להזמין באותו זמן אבל בשולחן אחר
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable2 = context.reserveTable(customer2);
            assertNotNull(reservedTable2);
            assertNotEquals(reservedTable.getId(), reservedTable2.getId());
        }
    }

    @Nested
    class IntegrationTests {
        @Test
        void testFullReservationFlow() {
            // 1. יוצר מסעדה חדשה
            List<Table> tables = new ArrayList<>();
            tables.add(new Table(1, 2));
            tables.add(new Table(2, 4));
            Restaurant newRestaurant = new Restaurant("New Restaurant", tables, 10, 22);

            // 2. יוצר קונטקסט עם אסטרטגיה אופטימלית
            TableReservationContext newContext = new TableReservationContext(
                    newRestaurant, new OptimizedTableReservationStrategy());

            // 3. מזמין שולחן
            Customer newCustomer = new Customer("New Customer", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable = newContext.reserveTable(newCustomer);
            assertNotNull(reservedTable);
            assertEquals(1, reservedTable.getId());

            // 4. מנסה להזמין שולחן נוסף
            Customer customer2 = new Customer("Customer 2", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable2 = newContext.reserveTable(customer2);
            assertNotNull(reservedTable2);
            assertEquals(2, reservedTable2.getId());

            // 5. מבטל הזמנה
            boolean cancelled = newContext.cancelReservation(newCustomer);
            assertTrue(cancelled);

            // 6. מנסה להזמין שוב את השולחן שהתפנה
            Customer customer3 = new Customer("Customer 3", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable3 = newContext.reserveTable(customer3);
            assertNotNull(reservedTable3);
            assertEquals(1, reservedTable3.getId());
        }

        @Test
        void testWaitlistFlow() {
            // יוצר קונטקסט לטסט
            TableReservationContext context = new TableReservationContext(
                    restaurant, new OptimizedTableReservationStrategy());

            // 1. יוצר לקוח כדי להזמין את השולחן הראשון
            Customer firstCustomer = new Customer("First", 2, DayOfTheWeek.MONDAY,
                    LocalTime.of(19, 0));
            Table reservedTable = context.reserveTable(firstCustomer);
            assertNotNull(reservedTable);

            // 2. מזמין את שאר השולחנות
            Customer customer2 = new Customer("Customer 2", 4, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer3 = new Customer("Customer 3", 6, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Customer customer4 = new Customer("Customer 4", 8, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            context.reserveTable(customer2);
            context.reserveTable(customer3);
            context.reserveTable(customer4);

            // 3. מנסה להצטרף לתור המתנה עם לקוח חדש
            Customer waitingCustomer = new Customer("Waiting", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            boolean joined = context.joinToWaitlist(waitingCustomer);
            assertTrue(joined);

            // 4. מבטל הזמנה של השולחן הראשון
            boolean cancelled = context.cancelReservation(firstCustomer);
            assertTrue(cancelled);

            // 5. בודק שהשולחן הראשון זמין שוב
            Customer newCustomer = new Customer("New Customer", 2, DayOfTheWeek.MONDAY, LocalTime.of(19, 0));
            Table newReservedTable = context.reserveTable(newCustomer);
            assertNotNull(newReservedTable);
            assertEquals(1, newReservedTable.getId());
        }
    }
}