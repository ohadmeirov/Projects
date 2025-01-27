package com.example.MyTrip.fragments.countriesFragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.inputmethod.InputMethodManager;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.example.MyTrip.R;

import java.util.ArrayList;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link FranceFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class FranceFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private RecyclerView recyclerViewFrance;

    public FranceFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FranceFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static FranceFragment newInstance(String param1, String param2) {
        FranceFragment fragment = new FranceFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_france, container, false);

        EditText editTextDays = view.findViewById(R.id.editTextDays);
        Button buttonShowTrips = view.findViewById(R.id.buttonShowTrips);
        recyclerViewFrance = view.findViewById(R.id.recyclerViewFrance);
        LinearLayout mainContainer = view.findViewById(R.id.mainContainer);
        ImageView imageView = view.findViewById(R.id.imageFrance);

        recyclerViewFrance.setLayoutManager(new LinearLayoutManager(getContext()));
        List<TripDetails> tripIdeas = new ArrayList<>();

        TripIdeasAdapter adapter = new TripIdeasAdapter(tripIdeas, mainContainer);
        recyclerViewFrance.setAdapter(adapter);

        buttonShowTrips.setOnClickListener(v -> {
            // Hide the flag
            imageView.setVisibility(View.GONE);

            // Hide the keyboard
            InputMethodManager imm = (InputMethodManager) requireContext().getSystemService(Context.INPUT_METHOD_SERVICE);
            imm.hideSoftInputFromWindow(editTextDays.getWindowToken(), 0);

            String daysInput = editTextDays.getText().toString().trim();
            if (!daysInput.isEmpty() && daysInput.matches("[1-7]")) {
                int days = Integer.parseInt(daysInput);
                tripIdeas.clear();
                tripIdeas.addAll(generateTripIdeas(days));
                adapter.notifyDataSetChanged();
            } else {
                Toast.makeText(getContext(), "Please enter a valid number between 1 and 7", Toast.LENGTH_SHORT).show();
            }
        });

        Glide.with(this).asGif().load(R.drawable.francewaveflag).into(imageView);

        return view;
    }

    private List<TripDetails> generateTripIdeas(int days) {
        List<TripDetails> ideas = new ArrayList<>();

        ideas.add(new TripDetails(
                "Day 1 - Paris", "יום 1 - פריז",
                "Begin your Parisian adventure with a ride on the HOP ON HOP OFF tour bus (Tickets below), the best way to get an overview of the city’s famous landmarks. This open-top bus allows you to explore Paris at your own pace. Key stops include:\n" +
                        "Eiffel Tower: Stop by the iconic Eiffel Tower, walk through the beautiful Champ de Mars gardens, and take photos of the world-famous landmark.\n" +
                        "Louvre Museum: Admire the grand architecture of the Louvre and explore its surroundings, such as the Tuileries Garden, even if you don’t venture inside.\n" +
                        "Notre-Dame Cathedral: See the magnificent Gothic structure from the outside, stroll along the Seine, and soak up the medieval charm of the Île de la Cité.\n" +
                        "Champs-Élysées & Arc de Triomphe: Explore the glamorous avenue, dotted with luxury boutiques, cafes, and theaters.\n" +
                        "In the evening, visit the Arc de Triomphe for panoramic views of Paris from its top. Witness the city's beauty as it lights up after dark, and take a leisurely walk down the bustling Champs-Élysées.\n"
                ,"התחל את ההרפתקה הפריזאית שלך בנסיעה באוטובוס הסיור HOP ON HOP OFF (כרטיסים למטה), הדרך הטובה ביותר לקבל סקירה כללית על ציוני הדרך המפורסמים של העיר. אוטובוס פתוח זה מאפשר לכם לחקור את פריז בקצב שלכם. עצירות מפתח כוללות:\n" +
                " מגדל אייפל: עצרו ליד מגדל אייפל האייקוני, צעדו דרך גני Champ de Mars היפים, וצלמו תמונות של ציון הדרך המפורסם בעולם.\n" +
                " מוזיאון הלובר: התפעל מהארכיטקטורה המפוארת של הלובר וחקור את סביבתו, כמו גן הטווילרי, גם אם אינך מסתכן בפנים.\n" +
                " קתדרלת נוטרדאם: ראה את המבנה הגותי המפואר מבחוץ, טיילו לאורך הסיין וספגו את הקסם של ימי הביניים של איל דה לה סיטה.\n" +
                " שאנז אליזה ושער הניצחון: חקור את השדרה הזוהרת, הזרועה בבוטיקים יוקרתיים, בתי קפה ותיאטראות.\n" +
                " בערב, בקרו בשער הניצחון לתצפיות פנורמיות על פריז מהפסגה. צפו ביופייה של העיר כשהיא מאירה לאחר רדת החשיכה, וצא לטיול נינוח בשאנז אליזה ההומה.", "https://www.tiqets.com/en/hop-on-hop-off-tours-paris-l205351/", "Hop On Hop Off Ticket link: "
        ));

        if (days > 1) {
            ideas.add(new TripDetails(
                    "Day 2 - Disneyland","יום 2 - דיסנילנד",
                    "Prepare for a day of magic at Disneyland Paris, easily accessible by taking the RER A train. Whether you’re traveling with family or reliving childhood dreams, the park offers an array of attractions, from thrilling rides to enchanting parades.\n" +
                            "Spend the morning exploring Disneyland Park, home to classic attractions like Space Mountain, Pirates of the Caribbean, and Sleeping Beauty Castle. In the afternoon, visit Walt Disney Studios Park, where you can learn about filmmaking, enjoy themed rides, and meet beloved Disney characters.\n" +
                            "End your day by watching the spectacular fireworks display over Sleeping Beauty Castle before heading back to Paris.\n"
                    ,"התכוננו ליום של קסם בדיסנילנד פריז, נגיש בקלות על ידי נסיעה ברכבת RER A. בין אם אתם מטיילים עם המשפחה או משחזרים חלומות ילדות, הפארק מציע מגוון אטרקציות, מטיולים מרתקים ועד מצעדים קסומים.\n \n" +
                    " בלו את הבוקר בחקר דיסנילנד פארק, ביתם של אטרקציות קלאסיות כמו הר החלל, שודדי הקאריביים וטירת היפהפייה הנרדמת. אחר הצהריים, בקרו בפארק אולפני וולט דיסני, שם תוכלו ללמוד על יצירת סרטים, ליהנות מטיולי נושא ולפגוש אהובים. דמויות דיסני.\n \n" +
                    " סיים את היום בצפייה במופע הזיקוקים המרהיב מעל טירת היפהפייה הנרדמת לפני שתחזור לפריז.","https://www.tiqets.com/en/search?q=Dysnilend%20paris", "Link to purchase tickets to Disneyland: "
            ));
        }

        if (days > 2) {
            ideas.add(new TripDetails(
                    "Day 3 - Shopping Day and Tour Eiffel","יום 3 - יום קניות ומגדל אייפל",
                    "Shopping day. Dedicate the second day to shopping! Take the RER A train to Val d’Europe, a massive shopping center near Disneyland Paris. This mall is a shopper’s paradise, featuring high-end brands, French boutiques, and plenty of dining options. For a luxury outlet experience, visit La Vallée Village, located nearby, where you can find designer items at discounted prices.\n" +
                            "Alternatively, consider exploring other shopping destinations:\n" +
                            "Centre Commercial Beaugrenelle: Located near the Eiffel Tower, this modern mall offers a variety of stores, restaurants, and even a cinema.\n" +
                            "Galeries Lafayette: Paris’ most famous department store, known for its luxury goods, fashion, and an incredible rooftop view of the city.\n" +
                            "In the evening, ascend the Eiffel Tower for breathtaking views of the city. The sparkling lights of Paris from above create an unforgettable experience. Tickets below",
                    "יום קניות. הקדישו את היום השני לקניות! קחו את רכבת ה-RER A ל-Val d'Europe, מרכז קניות ענק ליד דיסנילנד פריז. הקניון הזה הוא גן עדן לקונים, הכולל מותגים יוקרתיים, בוטיקים צרפתיים ושפע של אפשרויות לסעודה. לחוויית אאוטלט יוקרתית, בקר בכפר La Vallée, הממוקם בקרבת מקום, שם תוכל למצוא פריטי מעצבים במחירים מוזלים.\n \n" +
                    " לחלופין, שקול לחקור יעדי קניות אחרים:\n \n" +
                    " Centre Commercial Beaugrenelle: ממוקם ליד מגדל אייפל, הקניון המודרני הזה מציע מגוון חנויות, מסעדות ואפילו בית קולנוע. \n" +
                    " גלרי לאפייט: חנות הכלבו המפורסמת ביותר של פריז, הידועה במוצרי היוקרה, האופנה ובנוף הגג המדהים של העיר.\n \n" +
                    " בערב, נעלה למגדל אייפל לתצפיות עוצרי נשימה של העיר. האורות הנוצצים של פריז מלמעלה יוצרים חוויה בלתי נשכחת. כרטיסים למטה.", "https://www.toureiffel.paris/en/rates-offers/ticket-second-floor-stairs?utm_source=magpietravelgttd", "Eiffel Tower tickets link: "
            ));
        }

        if (days > 3) {
            ideas.add(new TripDetails(
                    "Day 4 - Palace of Versailles","יום 4 - ארמונות וורסאי",
                    "Step back in time with a visit to the Palace of Versailles (Tickets below), an iconic symbol of French history and opulence. Take the RER C train to Versailles and begin your journey by exploring the grand Hall of Mirrors, the Royal Apartments, and the beautifully manicured Versailles Gardens.\n" +
                            "Stroll through the gardens, admire the fountains, and don’t miss the Grand Trianon and Marie Antoinette’s Estate. Pack a picnic or enjoy lunch at one of the cafes within the palace grounds, In addition, I recommend to purchase a vehicle to drive inside the gardens. Spend the afternoon soaking in the lavish atmosphere before heading back to Paris.","צעד אחורה בזמן עם ביקור בארמון ורסאי (כרטיסים למטה), סמל איקוני של ההיסטוריה והשפע הצרפתי. קח את רכבת ה-RER C לוורסאי והתחיל את המסע על ידי סיור באולם המראות הגדול, הדירות המלכותיות וגני ורסאי המטופחים להפליא.\n" +
                    " טיילו בגנים, התפעלו מהמזרקות ואל תפספסו את גרנד טריאנון ואת אחוזת מארי אנטואנט. ארזו פיקניק או תהנו מארוחת צהריים באחד מבתי הקפה בשטח הארמון, בנוסף, אני ממליץ לרכוש רכב לנהיגה. בתוך הגנים בלו את אחר הצהריים לטבול באווירה המפוארת לפני שתחזרו לפריז.", "https://www.tiqets.com/en/versailles-attractions-c66594/", "Link to purchase Palace of Versailles entrance tickets: "
            ));
        }

        if (days > 4) {
            ideas.add(new TripDetails(
                    "Day 5 - Montmartre and Seine River Cruise","יום 5 - מונמרט ושייט בסיין ",
                    "Start your morning with a visit to Montmartre, the bohemian heart of Paris. Take a taxi to the district, as it’s perched on a hill. Begin your exploration at Sacré-Cœur Basilica, where you’ll enjoy stunning panoramic views of the city from the steps of the basilica.\n" +
                            "Wander through the charming cobblestone streets, discovering quaint cafes, art galleries, and the famous Place du Tertre, where artists display their work. Stop for a coffee or crepe at one of the local bistros.\n" +
                            "In the evening, head to the Seine River for a magical boat cruise (Tickets below). Float past illuminated landmarks such as the Eiffel Tower, Notre-Dame Cathedral, and the Louvre as you enjoy the romantic ambiance of the city at night.","התחילו את הבוקר שלכם בביקור במונמארטר, הלב הבוהמייני של פריז. קח מונית לרובע, שכן הוא יושב על גבעה. התחל את החקירה שלך בבזיליקת Sacré-Cœur, שם תיהנה מנופים פנורמיים מדהימים של העיר מהמדרגות של הבזיליקה.\n\n" +
                    " שוטט ברחובות המרוצפים המקסימים, גלה בתי קפה מוזרים, גלריות אמנות וכיכר טרטר המפורסמת, שם מציגים אמנים את יצירותיהם. עצרו לשתות קפה או קרפ באחד מהביסטרו המקומיים.\n \n" +
                    " בערב, צאו לנהר הסיין לשייט קסום בסירה (כרטיסים למטה). צפו על פני נקודות ציון מוארות כמו מגדל אייפל, קתדרלת נוטרדאם והלובר כשאתם נהנים מהאווירה הרומנטית של העיר בלילה.","https://www.tiqets.com/en/search?q=seine%20river", "Link to purchase Seine River Cruise tickets: "
            ));
        }

        if (days > 5) {
            ideas.add(new TripDetails(
                    "Day 6 - Champs-Élysées and Giverny","יום 6 - שנז אליזה וגיברני",
                    "Begin your day with a stroll along the Champs-Élysées, Paris’ most famous avenue. Browse the luxury shops, stop by Ladurée for their signature macarons, and take in the lively atmosphere. You can enjoy the stunning views of the Arc de Triomphe at the western end of the avenue, or take a moment to relax at one of the many cafés lining the street.\n" +
                            "For an unforgettable experience outside Paris, take a day trip to Giverny, the home of Claude Monet. I recommend to take a train, tickets below, which is in Paris St-Lazare station.  Start your visit with a tour of Monet’s House and Gardens, where you’ll find the picturesque water lilies and the iconic Japanese bridge that inspired many of his famous paintings. The house itself is a beautiful reflection of Monet’s life and style, filled with vibrant colors and works of art. Spend some time in the lush gardens, including the famous flower garden and the tranquil water garden that are a testament to Monet’s love for nature.\n" +
                            "While in Giverny, don’t miss the Musée des Impressionnismes, an Impressionist museum that showcases works by artists influenced by Monet and offers temporary exhibitions that explore the evolution of the movement. Take a walk through the charming village of Giverny, where the picturesque streets and vibrant homes will make you feel like you’ve stepped into a painting. You can also visit the Church of Saint-Radegonde, where Monet’s family is buried, adding a touch of history to your trip.\n" +
                            "Before returning to Paris, make sure to explore the local artisan shops, where you can find unique works of art, crafts, and products inspired by the village’s artistic legacy. This day trip will leave you with a deeper appreciation for both Monet and the tranquil beauty of Giverny.","התחילו את היום בטיול לאורך השאנז אליזה, השדרה המפורסמת ביותר של פריז. עיין בחנויות היוקרה, עצרו ליד Ladurée עבור המקרונים היחודיים שלהם, והטמיעו את האווירה התוססת. אתה יכול ליהנות מהנופים המדהימים של שער הניצחון בקצה המערבי של השדרה, או לקחת רגע להירגע באחד מבתי הקפה הרבים לאורך הרחוב.\n\n" +
                    " לחוויה בלתי נשכחת מחוץ לפריז, צאו לטיול יום לג'יברני, ביתו של קלוד מונה. אני ממליץ לקחת רכבת, כרטיסים למטה, שנמצאת בתחנת פריז סנט לזאר. התחל את הביקור שלך בסיור בבית מונה ו גנים, שבהם תמצאו את חבצלות המים הציוריות ואת הגשר היפני האיקוני שהיוו השראה לרבים מציוריו המפורסמים הבית עצמו הוא השתקפות יפה של חייו וסגנונו של מונה צבעים מרהיבים ויצירות אמנות בלו קצת זמן בגנים השופעים, כולל גן הפרחים המפורסם וגן המים השליו שהם עדות לאהבתו של מונה לטבע.\n \n" +
                    " בזמן שהייה בג'יברני, אל תחמיצו את מוזיאון אימפרסיוניזם, מוזיאון אימפרסיוניסטי המציג יצירות של אמנים שהושפעו ממונה ומציע תערוכות זמניות החוקרים את התפתחות התנועה. צאו לטיול דרך הכפר המקסים ג'יברני, שבו הציורי רחובות ובתים תוססים יגרמו לך להרגיש כאילו נכנסת לציור. אתה יכול גם לבקר בכנסיית Saint-Radegonde, שבה משפחתו של מונה קבור, ומוסיף נופך של היסטוריה לטיול שלך.\n \n" +
                    " לפני החזרה לפריז, הקפידו לחקור את חנויות האומנים המקומיות, בהן תוכלו למצוא יצירות אמנות, אמנות ומוצרים ייחודיים בהשראת המורשת האמנותית של הכפר. טיול יום זה ישאיר אתכם עם הערכה עמוקה יותר הן למונה והן ל- היופי השליו של ג'יברני.","https://www.thetrainline.com/?journeySearchType=single&destination=urn%3Atrainline%3Ageneric%3Aloc%3A1943&gad_source=1&gclid=Cj0KCQiA7se8BhCAARIsAKnF3ry_wc7-StY-TPOtmZPZ3PV6qNp6sAQkQ9f6V4rHBosE0o3uLyH_ioEaAttDEALw_wcB", "Link to purchase train tickets: "
            ));
        }

        if (days > 6) {
            ideas.add(new TripDetails(
                    "Day 7 - A Walking Tour of Paris and the Louvre Museum","יום 7 - יום הליכה בפריז ומוזיאון הלובר",
                    "On your final day, enjoy a walking tour of Paris, starting with the Notre-Dame Cathedral. While the interior is under restoration, you can admire its Gothic exterior and explore the surrounding Île de la Cité.\n" +
                            "From there, visit the Luxembourg Gardens, a peaceful retreat in the heart of the city. Wander through the beautifully landscaped gardens, relax by the fountains, and soak up the Parisian atmosphere.\n" +
                            "Next, head to Sainte-Chapelle, a hidden gem famous for its stunning stained glass windows. Cross the Pont Neuf to explore Conciergerie, a historic site once used as a prison during the French Revolution.\n" +
                            "End your day at the Louvre Museum (Tickets below), where you can spend hours admiring world-class art collections, including the Mona Lisa. If time allows, take a leisurely walk along the Seine or enjoy dinner at a nearby restaurant to savor your last evening in Paris.\n" +
                            "This seven-day itinerary combines Paris’ iconic landmarks, shopping, culture, and history with a touch of adventure outside the city, ensuring a memorable trip filled with diverse experiences.",
                    "ביום האחרון שלך, תהנה מסיור רגלי בפריז, החל בקתדרלת נוטרדאם. בעוד החלק הפנימי נמצא בשיקום, אתה יכול להתפעל מבחוץ הגותי שלו ולחקור את איל דה לה סיטה שמסביב.\n \n" +
                            " משם, בקרו בגני לוקסמבורג, מקום מפלט שליו בלב העיר. לשוטט בגנים המעוצבים להפליא, להירגע ליד המזרקות ולספוג את האווירה הפריזאית.\n \n" +
                            " לאחר מכן, לכו לסנט-שאפל, פנינה נסתרת המפורסמת בזכות חלונות הוויטראז' המהממים שלה. חצו את Pont Neuf כדי לחקור את קונסיירג'רי, אתר היסטורי ששימש בעבר כבית סוהר במהלך המהפכה הצרפתית.\n \n" +
                            " סיים את היום שלך במוזיאון הלובר (כרטיסים למטה), שם תוכל לבלות שעות בהתפעלות מאוספי אמנות ברמה עולמית, כולל המונה ליזה. אם הזמן מאפשר, צאו לטיול נינוח לאורך הסיין או תהנו מארוחת ערב במסעדה סמוכה כדי להתענג על הערב האחרון שלך בפריז.\n \n" +
                            " מסלול זה בן שבעה ימים משלב את ציוני הדרך האיקוניים של פריז, קניות, תרבות והיסטוריה עם מגע של הרפתקאות מחוץ לעיר, מה שמבטיח טיול בלתי נשכח מלא בחוויות מגוונות.", "https://www.tiqets.com/en/louvre-museum-tickets-l124297/", "Link to buy the tickets to the Louvre Museum: "
            ));
        }
        return ideas;
    }
}