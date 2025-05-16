package com.hit.algorithm;

import com.hit.model.*;

// ממשק זה מגדיר את החוזה לכל אסטרטגיית הזמנת שולחנות (נאיבי, אופטימלי וכו')
public interface IAlgoTableReservationStrategy {
    /**
     * מנסה להזמין שולחן עבור לקוח במסעדה
     * @param restaurant המסעדה
     * @param customer הלקוח
     * @return Table אם נמצא שולחן, או null אם לא
     */
    Table reserveTable(Restaurant restaurant, Customer customer);

    /**
     * מבטל הזמנת שולחן של לקוח במסעדה
     * @param restaurant המסעדה
     * @param customer הלקוח
     * @return true אם ההזמנה בוטלה בהצלחה, false אחרת
     */
    boolean cancelReservation(Restaurant restaurant, Customer customer);

    /**
     * מוסיף לקוח לתור המתנה במסעדה
     * @param restaurant המסעדה
     * @param customer הלקוח
     * @return true אם הלקוח נוסף לתור בהצלחה, false אחרת
     */
    boolean joinToWaitlist(Restaurant restaurant, Customer customer);
}