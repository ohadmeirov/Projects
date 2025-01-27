
package com.example.MyTrip.fragments.countriesFragments;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;
import android.view.inputmethod.InputMethodManager;
import android.content.Context;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.example.MyTrip.R;
import java.util.ArrayList;
import java.util.List;

public class CzechRepublicFragment extends Fragment {
    private RecyclerView recyclerViewCzechRepublic;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_czech_republic, container, false);

        EditText editTextDays = view.findViewById(R.id.editTextDays);
        Button buttonShowTrips = view.findViewById(R.id.buttonShowTrips);
        recyclerViewCzechRepublic = view.findViewById(R.id.recyclerViewCzechRepublic);
        LinearLayout mainContainer = view.findViewById(R.id.mainContainer);
        ImageView imageView = view.findViewById(R.id.imageCzechRepublic);

        recyclerViewCzechRepublic.setLayoutManager(new LinearLayoutManager(getContext()));

        List<TripDetails> tripIdeas = new ArrayList<>();
        TripIdeasAdapter adapter = new TripIdeasAdapter(tripIdeas, mainContainer);
        recyclerViewCzechRepublic.setAdapter(adapter);

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

        Glide.with(this)
                .asGif()
                .load(R.drawable.czechrepublicwaveflag)
                .into(imageView);

        return view;
    }

    private List<TripDetails> generateTripIdeas(int days) {
        List<TripDetails> ideas = new ArrayList<>();

        ideas.add(new TripDetails(
                "Day 1 - Prague","יום 1 - פראג",
                "First of all, I personally recommend starting your day in Prague with a HOP ON HOP OFF tour. You can purchase your tickets via the link below. At HOP ON HOP OFF, Station 42 covers all the main points of the city. Here's a brief overview of some key stops:\n\n" +
                        "1. Dlabačov: Located near Strahov Hill and the Strahov Gardens, offering a fantastic panoramic view of Prague. Great for starting your journey from an elevated spot.\n" +
                        "2. Pohořelec: Close to Strahov Monastery with a famous library and magnificent halls. Perfect for history and architecture lovers.\n" +
                        "3. Brusnice: Near the Royal Gardens, a peaceful area ideal for a quiet walk.\n" +
                        "4. Pražský hrad (Prague Castle): The main site, including St. Vitus Cathedral, the Old Royal Palace, and the Royal Gardens. Must-visit.\n" +
                        "5. Královský letohrádek (Summer Palace): A lovely summer palace with beautiful gardens, perfect for a picnic.\n" +
                        "6. Malostranská: Close to Charles Bridge and Lesser Town. Wander through picturesque streets and visit St. Nicholas Church.\n" +
                        "7. Právnická fakulta: Near the historic university and Charles Bridge. Great for crossing into the Old Town.\n" +
                        "8. Čechův most (Čech Bridge): A modern bridge offering a charming view of the river and city.\n" +
                        "9. Náměstí Republiky (Republic Square): Near shopping centers like Palladium and the Powder Tower. Combine shopping with historical sites.\n" +
                        "10. Masarykovo nádraží (Masaryk Station): Historic station connecting to the business district and the New Town.\n" +
                        "11. Dlouhá třída: Near the Jewish Quarter and boutique shops.\n" +
                        "12. Jindřišská: Close to Jindřišská Tower and Wenceslas Square.\n" +
                        "13. Václavské náměstí (Wenceslas Square): A major square with shops, restaurants, and museums.\n" +
                        "14. Vodičkova: A lively street with cafes and restaurants.\n" +
                        "15. Lazarská: A great place for dinner or a coffee break.\n" +
                        "16. Národní třída: Close to the National Theatre.\n" +
                        "17. Jiráskovo náměstí: Near the Dancing House on the Vltava River, a perfect spot for a photo.\n" +
                        "18. Újezd: Near the Petřín Hill funicular for a panoramic view.\n" +
                        "19. Hellichova: Close to Kampa Island parks for a relaxing walk.\n" +
                        "20. Malostranské náměstí (Lesser Town Square): A central stop in Lesser Town, great for exploring narrow streets and crossing Charles Bridge." +
                        "Summary: Some of the most recommended stops for an in-depth tour include Prague Castle (4), Old Town Square (8), Wenceslas Square (13), Charles Bridge (6, 7), the Dancing House (17), and Petřín Hill (18). If you have time in the evening, take a walk around your hotel area and head towards the Astronomical Clock. "
                ,"קודם כל, אני אישית ממליץ להתחיל את היום שלך בפראג עם סיור HOP ON HOP OFF. אתה יכול לרכוש את הכרטיסים שלך דרך הקישור למטה. ב-HOP ON HOP OFF, תחנה 42 מכסה את כל הנקודות המרכזיות של העיר. הנה סקירה קצרה של כמה עצירות מפתח: \n" +
                " \n1. Dalbac'hov: ממוקם ליד גבעת סטראחוב וגני סטראחוב, ומציע נוף פנורמי פנטסטי של פראג. מצוין להתחיל את המסע שלך מנקודה מוגבהת.\n" +
                " \n2. Pohořelec: קרוב למנזר Strahov עם ספרייה מפורסמת ואולמות מפוארים. מושלם לאוהבי היסטוריה וארכיטקטורה.\n" +
                " \n3. ברוסניצה: ליד הגנים המלכותיים, אזור שליו אידיאלי לטיול שקט.\n" +
                " \n4. Pražský hrad (מצודת פראג): האתר המרכזי, כולל קתדרלת סנט ויטוס, הארמון המלכותי הישן והגנים המלכותיים. חובה לבקר.\n" +
                " \n5. Královský letohrádek (ארמון הקיץ): ארמון קיץ מקסים עם גנים יפים, מושלם לפיקניק.\n" +
                " \n6. Malostranská: קרוב לגשר קארל ולסר טאון. לשוטט ברחובות ציוריים ולבקר בכנסיית סנט ניקולס.\n" +
                " \n7. Právnická fakulta: ליד האוניברסיטה ההיסטורית וגשר קארל. מצוין למעבר לעיר העתיקה.\n" +
                " \n8. Čechův most (גשר צ'ך): גשר מודרני המציע נוף מקסים של הנהר והעיר.\n" +
                " \n9. Náměstí Republiky (כיכר הרפובליקה): ליד מרכזי קניות כמו פלדיום ומגדל האבקה. שלבו קניות עם אתרים היסטוריים.\n" +
                " \n10. Masarykovo nádraží (תחנת מסריק): תחנה היסטורית המחברת לרובע העסקים ולעיר החדשה.\n" +
                " \n11. Dlouhá třída: ליד הרובע היהודי וחנויות בוטיק.\n" +
                " \n12. Jindřišská: קרוב למגדל Jindřišská ולכיכר ואצלב.\n" +
                " \n13. Václavské náměstí (כיכר ואצלב): כיכר מרכזית עם חנויות, מסעדות ומוזיאונים.\n" +
                " \n14. Vodičkova: רחוב תוסס עם בתי קפה ומסעדות.\n" +
                " \n15. Lazarská: מקום נהדר לארוחת ערב או הפסקת קפה.\n" +
                " \n16. Národní třída: קרוב לתיאטרון הלאומי.\n" +
                " \n17. Jiráskovo náměstí: ליד הבית הרוקד על נהר הוולטאבה, מקום מושלם לצילום.\n" +
                " \n18. Újezd: ליד הרכבל של גבעת פטרין לתצפית פנורמית.\n" +
                " \n19. Hellichova: קרוב לפארקים של האי קמפה לטיול מרגיע.\n" +
                " \n20. Malostranské náměstí (כיכר העיר הקטנה): תחנה מרכזית בלסר טאון, נהדרת לחקר רחובות צרים ולחציית גשר קארל.\n" +
                " \nסיכום: כמה מהעצירות המומלצות ביותר לסיור מעמיק כוללות את מצודת פראג (4), כיכר העיר העתיקה (8), כיכר ואצלב (13), גשר קארל (6, 7), הבית הרוקד (17), וגבעת Petřín (18) אם יש לכם זמן בערב, צאו לטייל באזור המלון שלכם וצאו לכיוון השעון האסטרונומי.\n", "https://www.tiqets.com/en/prague-hop-on-hop-off-l212158/", "Hop On Hop Off Line 42 Ticket link: "
        ));

        if (days > 1) {
            ideas.add(new TripDetails(
                    "Day 2 - Karlovy Vary","יום 2 - קלובי ווארי",
                    "Karlovy Vary is one of the most beautiful towns in the Czech Republic. First, I recommend purchasing a bus ticket to get there, as it is faster and more accessible than the train (and cheaper).\n" +
                            "I recommend on taking bus, its a lot cheaper and faster than train. The bus departs from the main bus station FLORENC in Prague and arrives at the TERMINAL in Karlovy Vary. Link Below. Once you get off the bus, the charm starts immediately with stunning views.\n\n" +
                            "Recommended places to visit in Karlovy Vary:\n" +
                            "1. *Hot Spring Colonnade (TUR DOLCENTER)*\n" +
                            "- Distance from Terminal: ~1.5 km (20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? A 19th-century colonnade surrounded by columns where you can drink from the hot spring waters.\n" +
                            "- Should you go inside? Yes, it's worth experiencing the unique mineral water.\n" +
                            "- How long to stay? 20-40 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "2. *Mill Colonnade (KOLONDA MIL)*\n" +
                            "- Distance from Hot Spring Colonnade: ~1 km (10 minutes walking or 3 minutes by taxi).\n" +
                            "- What is there? A colonnade with five hot springs.\n" +
                            "- Should you go inside? Yes, it's worth experiencing the hot spring waters.\n" +
                            "- How long to stay? 20-30 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "3. *Vřídelní kolonáda (KOLONDA WIDL)*\n" +
                            "- Distance from Mill Colonnade: ~200 meters (2 minutes walking).\n" +
                            "- What is there? A famous colonnade with a very hot spring at 72°C, also with a panoramic viewing point.\n" +
                            "- Should you go inside? Yes, it's worth drinking the hot spring water.\n" +
                            "- How long to stay? 15-30 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "4. *Diana Lookout Tower (MIGDAL TETSAPIT DIANA)*\n" +
                            "- Distance from Vřídelní kolonáda: ~1.5 km (20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? A high tower offering stunning views of the city. You can also reach it by cable car.\n" +
                            "- Should you go inside? Yes, it's worth experiencing the panoramic views.\n" +
                            "- How long to stay? 30-60 minutes.\n" +
                            "- Is there an entrance fee? Yes, there is an entrance fee.\n\n" +
                            "5. *Karlovy Vary City Park (GAN IR KARLOVI VARY)*\n" +
                            "- Distance from Diana Lookout Tower: ~500 meters (5 minutes walking).\n" +
                            "- What is there? A beautiful city park with well-maintained paths, gardens, and resting areas. Ideal for a short walk or relaxation.\n" +
                            "- Should you go inside? Yes, if you love nature and green spaces.\n" +
                            "- How long to stay? 30-60 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "6. *Loket Castle (HATIRAT LOCKET)*\n" +
                            "- Distance from Terminal: ~12 km (15 minutes by taxi or 20 minutes by train).\n" +
                            "- What is there? A medieval castle on a hill offering magnificent views of the region.\n" +
                            "- Should you go inside? Yes, it's worth visiting to explore the castle and its history.\n" +
                            "- How long to stay? 60-90 minutes.\n" +
                            "- Is there an entrance fee? Yes, there is an entrance fee.\n\n" +
                            "7. *Vřídlo Fountain (MAAYEN MIVABA)*\n" +
                            "- Distance from Hot Spring Colonnade: 5 minutes walking.\n" +
                            "- What is there? A bubbling spring that adds to the charm of Karlovy Vary.\n" +
                            "- Should you go inside? Yes, it's a great spot to witness the thermal waters.\n" +
                            "- How long to stay? 10-20 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "8. *Petrin Hill (HAR PETRIN)*\n" +
                            "- Distance from Terminal: ~3 km (30 minutes walking or 10 minutes by taxi).\n" +
                            "- What is there? A natural area with walking trails and a park offering stunning views of the city.\n" +
                            "- Should you go inside? Yes, if you enjoy nature and panoramic views.\n" +
                            "- How long to stay? 60-120 minutes.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "9. *Karlovy Vary Christmas Market (SHUK CHAG HAMOLAD KERLOVI VARI)*\n" +
                            "- Distance from Terminal: ~1.5 km (15-20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? A charming Christmas market with stalls offering food, handmade goods, decorations, and gifts, creating a festive atmosphere.\n" +
                            "- Should you go inside? Yes, especially if you visit during the holiday season.\n" +
                            "- How long to stay? 30-60 minutes.\n" +
                            "- Is there an entrance fee? No, but you will need money for purchases.\n\n" +
                            "10. *Return to Terminal (CHAZARA L'TERMINAL)*\n" +
                            "- Distance from Terminal: ~1.5 km (20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? Return to the city center to continue your journey or plan your next day.\n","קרלובי וארי היא אחת הערים היפות בצ'כיה. ראשית, אני ממליץ לרכוש כרטיס אוטובוס כדי להגיע לשם, מכיוון שהוא מהיר ונגיש יותר מהרכבת (וזול יותר).\n" +
                    " \"אני ממליץ לקחת אוטובוס, זה הרבה יותר זול ומהיר מרכבת. האוטובוס יוצא מתחנת האוטובוסים המרכזית FLORENC בפראג ומגיע לטרמינל בקרלובי וארי. קישור למטה. ברגע שאתה יורד מהאוטובוס, הקסם מתחיל מיד עם נופים מדהימים.\n" +
                    "\nמקומות מומלצים לביקור בקרלובי וארי:\n" +
                    "\n1. *מדת עמודים מעיינות חמה (TUR DOLCENTER)*\n" +
                    "- מרחק מהטרמינל: ~1.5 ק\"מ (20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? עמוד עמודים מהמאה ה-19 מוקף עמודים שבהם אפשר לשתות ממי המעיינות החמים.\n" +
                    "- כדאי להיכנס פנימה? כן, כדאי להתנסות במים המינרלים הייחודיים.\n" +
                    "- כמה זמן להישאר? 20-40 דקות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    " \n2. *Mill Colonnade (KOLONDA MIL)*\n" +
                    "- מרחק מ-Hot Spring Colonnade: ~1 ק\"מ (10 דקות הליכה או 3 דקות במונית).\n" +
                    "- מה יש שם? קולונדה עם חמישה מעיינות חמים.\n" +
                    "- כדאי להיכנס פנימה? כן, שווה לחוות את מי המעיינות החמים.\n" +
                    "- כמה זמן להישאר? 20-30 דקות.\n" +
                    " - האם יש דמי כניסה? לא.\n" +
                    " \n3. *Vřídelní kolonáda (KOLONDA WIDL)*\n" +
                    " - מרחק מ-Mill Colonnade: ~200 מטר (2 דקות הליכה).\n" +
                    "- מה יש שם? קולונדה מפורסמת עם מעיין חם מאוד ב-72°C, גם עם נקודת תצפית פנורמית.\n" +
                    " - כדאי להיכנס פנימה? כן, כדאי לשתות את מי המעיינות החמים.\n" +
                    " - כמה זמן להישאר? 15-30 דקות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    "\n4. *מגדל תצפית דיאנה (MIGDAL TETSAPIT DIANA)*\n" +
                    "- מרחק מ-Vřídelní kolonáda: ~1.5 ק\"מ (20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? מגדל גבוה המציע נופים מדהימים של העיר. אפשר להגיע אליו גם ברכבל.\n" +
                    "- כדאי להיכנס פנימה? כן, שווה לחוות את הנופים הפנורמיים.\n" +
                    "- כמה זמן להישאר? 30-60 דקות.\n" +
                    "- האם יש דמי כניסה? כן, יש דמי כניסה.\n" +
                    "\n.5 *פארק העיר קרלובי וארי (GAN IR KARLOVI VARY)*\n" +
                    "- מרחק ממגדל תצפית דיאנה: ~500 מטר (5 דקות הליכה).\n" +
                    "- מה יש שם? פארק עירוני יפהפה עם שבילים, גנים ואזורי מנוחה מטופחים. אידיאלי לטיול קצר או מנוחה.\n" +
                    "- האם כדאי להיכנס פנימה? כן, אם אתה אוהב טבע ומרחבים ירוקים.\n" +
                    "- כמה זמן להישאר? 30-60 דקות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    "\n.6 *טירת לוקט (HATIRAT LOCKET)*\n" +
                    "- מרחק מהטרמינל: ~12 ק\"מ (15 דקות במונית או 20 דקות ברכבת).\n" +
                    "- מה יש שם? טירה מימי הביניים על גבעה המציעה נופים מרהיבים של האזור.\n" +
                    "- כדאי להיכנס פנימה? כן, כדאי לבקר כדי לחקור את הטירה וההיסטוריה שלה.\n" +
                    "- כמה זמן להישאר? 60-90 דקות.\n" +
                    "- האם יש דמי כניסה? כן, יש דמי כניסה.\n" +
                    "\n7. *מזרקת Vřídlo (MAAYEN MIVABA)*" +
                    "- מרחק מ-Hot Spring Colonnade: 5 דקות הליכה.\n" +
                    "- מה יש שם? מעיין מבעבע שמוסיף לקסם של קרלובי וארי.\n" +
                    "- כדאי להיכנס פנימה? כן, זה מקום נהדר לראות את המים התרמיים.\n" +
                    "- כמה זמן להישאר? 10-20 דקות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    " \n8. *Petrin Hill (HAR PETRIN)*\n" +
                    "- מרחק מהטרמינל: ~3 ק\"מ (30 דקות הליכה או 10 דקות במונית).\n" +
                    "- מה יש שם? אזור טבעי עם שבילי הליכה ופארק שמציע נופים מדהימים של העיר.\n" +
                    "- האם כדאי להיכנס פנימה? כן, אם אתה נהנה מהטבע ומנוף פנורמי.\n" +
                    "- עד מתי שָׁהוּת? 60-120 דקות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    "\n9. *שוק חג המולד של קרלובי וארי במידה והגעתם בחורף (SHUK CHAG HAMOLAD KERLOVI VARI)*\n" +
                    "- מרחק מהטרמינל: ~1.5 ק\"מ (15-20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? שוק חג מולד מקסים עם דוכנים המציעים אוכל, מוצרים בעבודת יד, קישוטים ומתנות, יוצרים אווירה חגיגית.\n" +
                    "- האם כדאי להיכנס פנימה? כן, במיוחד אם אתה מבקר במהלך עונת החגים.\n" +
                    "- כמה זמן להישאר? 30-60 דקות.\n" +
                    "- האם יש דמי כניסה? לא, אבל תצטרך כסף לרכישות.\n" +
                    "\n10. *חזור למסוף (CHAZARA L'TERMINAL)*\n" +
                    "- מרחק מהטרמינל: ~1.5 ק\"מ (20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש? חזור למרכז העיר כדי להמשיך את המסע או לתכנן את היום הבא.\n",
                    "https://regiojet.com/", "Link to purchase bus tickets: "
            ));
        }

        if (days > 2) {
            ideas.add(new TripDetails(
                    "Day 3 - Shopping Day", "יום 3 - יום קניות",
                    "Shopping day. I recommend visiting the Chodov Mall, the Fashion Arena Outlet, and if there's time, end the day at Palladium Mall. Outside the mall, you can also stroll around the Old Town at night, and there are shopping centers near Wenceslas Square as well.",
                    "יום קניות. ממליץ לבקר בקניון חודוב, באאוטלט פאשן ארנה, ואם יש זמן לסיים את היום בקניון פלדיום. מחוץ לקניון אפשר גם לטייל בעיר העתיקה בלילה, ויש מרכזי קניות ליד כיכר ואצלב.\n", "", "Enjoy your Shopping Today!"
            ));
        }

        if (days > 3) {
            ideas.add(new TripDetails(
                    "Day 4 - Český Krumlov", "יום 4 - צ'סקי קרומלוב",
                    "Český Krumlov is a UNESCO World Heritage site with a stunning medieval center. It's one of the most beautiful towns in the Czech Republic.\n\n" +
                            "I recommend on taking bus, its a lot cheaper and faster than train. The bus departs from the main bus station FLORENC in Prague. Link Below."+
                            "Recommended places to visit in Český Krumlov:\n" +
                            "1. *Český Krumlov Castle*\n" +
                            "- Distance from the town center: ~10 minutes walking.\n" +
                            "- What is there? The castle complex with a tower offering breathtaking views.\n" +
                            "- Should you go inside? Yes, it's the highlight of the town.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? Yes, there is an entrance fee.\n\n" +
                            "2. *Old Town*\n" +
                            "- Distance from the castle: ~5 minutes walking.\n" +
                            "- What is there? A beautifully preserved medieval old town with charming streets and architecture.\n" +
                            "- Should you go inside? Yes, it's perfect for a stroll.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "3. *Vltava River*\n" +
                            "- Distance from the Old Town: ~10 minutes walking.\n" +
                            "- What is there? You can take a boat ride or walk along the river.\n" +
                            "- Should you go inside? Yes, the river offers a lovely view of the town and the castle.\n" +
                            "- How long to stay? 30 minutes to 1 hour.\n" +
                            "- Is there an entrance fee? No.\n\n" +
                            "4. *Regional Museum*\n" +
                            "- Distance from the Old Town: ~10 minutes walking.\n" +
                            "- What is there? A museum showcasing the history of the region.\n" +
                            "- Should you go inside? Yes, it's a great way to learn about Český Krumlov's history.\n" +
                            "- How long to stay? 30-45 minutes.\n" +
                            "- Is there an entrance fee? Yes, there is an entrance fee.",
                    "צ'סקי קרומלוב היא אתר מורשת עולמית של אונסקו עם מרכז מרהיב מימי הביניים. זוהי אחת הערים היפות ביותר בצ'כיה.\n" +
                    "אני ממליץ לקחת אוטובוס, זה הרבה יותר זול ומהיר מרכבת. האוטובוס יוצא מתחנת האוטובוס המרכזית FLORENC בפראג. קישור למטה.\n" +
                    "מקומות מומלצים לביקור בצ'סקי קרומלוב:\n" +
                    "\n1. *טירת צ'סקי קרומלוב*\n" +
                    "- מרחק ממרכז העיר: ~10 דקות הליכה.\n" +
                    "- מה יש שם? מתחם הטירה עם מגדל המציע נופים עוצרי נשימה.\n" +
                    "- כדאי להיכנס פנימה? כן, זה גולת הכותרת של העיר.\n" +
                    "- כמה זמן להישאר? 1-2 שעות.\n" +
                    "- האם יש דמי כניסה? כן, יש דמי כניסה.\n" +
                    "\n2. *עיר עתיקה*\n" +
                    "- מרחק מהטירה: ~5 דקות הליכה.\n" +
                    "- מה יש שם? עיר עתיקה מימי הביניים שמורה להפליא עם רחובות מקסימים וארכיטקטורה.\n" +
                    "- האם כדאי להיכנס פנימה? כן, זה מושלם לטיול.\n" +
                    "- כמה זמן להישאר? 1-2 שעות.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    "\n3. *נהר הוולטאבה*\n" +
                    "- מרחק מהעיר העתיקה: ~10 דקות הליכה.\n" +
                    "- מה יש שם? אתה יכול לקחת שייט בסירה או ללכת לאורך הנהר.\n" +
                    "- כדאי להיכנס פנימה? כן, הנהר מציע נוף מקסים של העיר והטירה.\n" +
                    "- כמה זמן להישאר? 30 דקות עד שעה.\n" +
                    "- האם יש דמי כניסה? לא.\n" +
                    "\n4. *מוזיאון אזורי*\n" +
                    "- מרחק מהעיר העתיקה: ~10 דקות הליכה.\n" +
                    "- מה יש שם? מוזיאון המציג את ההיסטוריה של האזור.\n" +
                    "- האם כדאי להיכנס פנימה? כן, זו דרך מצוינת ללמוד על ההיסטוריה של צ'סקי קרומלוב.\n" +
                    " - כמה זמן להישאר? 30-45 דקות.\n" +
                    " - האם יש דמי כניסה? כן, יש דמי כניסה.\n", "https://regiojet.com/", "Link to purchase bus tickets: "
            ));
        }

        if (days > 4) {
            ideas.add(new TripDetails(
                    "Day 5 - Dresden", "יום 5 - דרזדן",
                    "Dresden is a beautiful city known for its art, architecture, and culture. The train ride from Prague to Dresden is quick and affordable, taking you to the heart of the city.\n\n" +
                            "I recommend to take a train its a lot faster and cheaper than a bus. the Train station is near the Vaslev square. Tickets Below. "+
                            "Recommended places to visit in Dresden:\n" +
                            "1. *Frauenkirche (Church of Our Lady)*\n" +
                            "- Distance from Dresden Hbf: ~1.5 km (20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? A magnificent Baroque church with a panoramic view.\n" +
                            "- Should you go inside? Yes, it’s worth seeing the magnificent interior and climbing to the panoramic view.\n" +
                            "- Entrance fee? Yes, there is an entrance fee for the view.\n" +
                            "- Time to stay? 30-60 minutes.\n\n" +
                            "2. *Neumarkt Square*\n" +
                            "- Distance from Frauenkirche: ~300 meters (4 minutes walking or 2 minutes by taxi).\n" +
                            "- What is there? A historic and charming square surrounded by Baroque architecture and cafes.\n" +
                            "- Should you go inside? Not necessary, it's great for a stroll.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? 20-30 minutes.\n\n" +
                            "3. *Zwinger Palace*\n" +
                            "- Distance from Neumarkt: ~600 meters (8 minutes walking or 3 minutes by taxi).\n" +
                            "- What is there? A historical palace with beautiful gardens.\n" +
                            "- Should you go inside? Yes, the gardens and courtyards are worth visiting.\n" +
                            "- Entrance fee? Yes, for the palace and internal museums.\n" +
                            "- Time to stay? 45-90 minutes.\n\n" +
                            "4. *Semperoper (Opera House)*\n" +
                            "- Distance from Zwinger Palace: ~300 meters (5 minutes walking or 2 minutes by taxi).\n" +
                            "- What is there? A stunning opera house with various performances.\n" +
                            "- Should you go inside? Yes, if there’s a performance, otherwise admire from the outside.\n" +
                            "- Entrance fee? Yes, for performances.\n" +
                            "- Time to stay? 15-30 minutes.\n\n" +
                            "5. *Brühl's Terrace*\n" +
                            "- Distance from Semperoper: ~500 meters (6 minutes walking or 3 minutes by taxi).\n" +
                            "- What is there? A scenic walkway with a view of the Elbe river.\n" +
                            "- Should you go inside? No, it’s an open area for a walk.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? 30-60 minutes.\n\n" +
                            "6. *Residenzschloss (Royal Palace)*\n" +
                            "- Distance from Brühl's Terrace: ~400 meters (5 minutes walking or 2 minutes by taxi).\n" +
                            "- What is there? A royal palace with museums and historical significance.\n" +
                            "- Should you go inside? Yes, it’s worth exploring.\n" +
                            "- Entrance fee? Yes.\n" +
                            "- Time to stay? 60-90 minutes.\n\n" +
                            "7. *Schloss Albrechtsberg*\n" +
                            "- Distance from Residenzschloss: ~2 km (25 minutes walking or 8 minutes by taxi).\n" +
                            "- What is there? A historic palace on a hill overlooking the Elbe river, with beautiful gardens.\n" +
                            "- Should you go inside? Yes, if there’s time, stroll through the gardens and the surroundings.\n" +
                            "- Entrance fee? Yes.\n" +
                            "- Time to stay? 60-90 minutes.\n\n" +
                            "8. *Fürstenzug (Procession of Princes)*\n" +
                            "- Distance from Schloss Albrechtsberg: ~1.5 km (20 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? A historic porcelain mural depicting the history of the Saxon royal family.\n" +
                            "- Should you go inside? No, it's an outdoor piece of art to admire from outside.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? 10-15 minutes.\n\n" +
                            "9. *Kunsthofpassage*\n" +
                            "- Distance from Fürstenzug: ~2.5 km (30 minutes walking or 10 minutes by taxi).\n" +
                            "- What is there? An artistic, colorful area with creative shops, galleries, and water/ music designed walls.\n" +
                            "- Should you go inside? Yes, it's worth seeing the unique and diverse art.\n" +
                            "- Entrance fee? No, though some galleries or shops may charge.\n" +
                            "- Time to stay? 30-60 minutes.\n\n" +
                            "10. *Großer Garten*\n" +
                            "- Distance from Kunsthofpassage: ~3 km (35 minutes walking or 12 minutes by taxi).\n" +
                            "- What is there? A vast park with walking/biking paths, beautiful gardens, and family attractions.\n" +
                            "- Should you go inside? Yes, enjoy the open park and relax.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? 60-120 minutes.\n\n" +
                            "11. *Dresden Christmas Market*\n" +
                            "- Distance from Großer Garten: ~3 km (35 minutes walking or 12 minutes by taxi).\n" +
                            "- What is there? One of the oldest Christmas markets in the world, with stalls offering traditional food, gifts, and hot drinks.\n" +
                            "- Should you go inside? Yes, it's a must-see during Christmas.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? 60-90 minutes.\n\n" +
                            "12. *Dresden Hbf (Central Train Station)*\n" +
                            "- Distance from Dresden Christmas Market: ~1 km (12 minutes walking or 5 minutes by taxi).\n" +
                            "- What is there? The central transportation hub of Dresden.\n" +
                            "- Should you go inside? Yes, if you’re continuing your journey.\n" +
                            "- Entrance fee? No.\n" +
                            "- Time to stay? Only if you're continuing your journey.","דרזדן היא עיר יפה הידועה באמנות, בארכיטקטורה ובתרבות שלה. הנסיעה ברכבת מפראג לדרזדן היא מהירה ובמחיר סביר, ומובילה אותך ללב העיר.\n" +
                    "אני ממליץ לקחת רכבת זה הרבה יותר מהיר וזול מאוטובוס. תחנת הרכבת נמצאת ליד כיכר ואסלב. כרטיסים למטה. \n" +
                    "מקומות מומלצים לביקור בדרזדן:\n" +
                    "\n1. *Frauenkirche (כנסיית גבירתנו)*\n" +
                    "- מרחק מדרזדן Hbf: ~1.5 ק\"מ (20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? כנסיית בארוק מפוארת עם נוף פנורמי.\n" +
                    "- כדאי להיכנס פנימה? כן, שווה לראות את הפנים המפואר ולטפס אל הנוף הפנורמי.\n" +
                    "- דמי כניסה? כן, יש דמי כניסה לנוף.\n" +
                    "- זמן להישאר? 30-60 דקות.\n" +
                    "\n. *כיכר ניומרקט*\n" +
                    "- מרחק מ-Frauenkirche: ~300 מטר (4 דקות הליכה או 2 דקות במונית).\n" +
                    "- מה יש שם? כיכר היסטורית ומקסימה מוקפת באדריכלות בארוק ובתי קפה.\n" +
                    "- האם כדאי להיכנס? לא הכרחי, זה נהדר לטיול.\n" +
                    "- דמי כניסה? לא.\n" +
                    "- זמן להישאר? 20-30 דקות.\n" +
                    "\n3. *ארמון צווינגר*\n" +
                    "- מרחק מנוימרקט: ~600 מטר (8 דקות הליכה או 3 דקות במונית).\n" +
                    "- מה יש שם? ארמון היסטורי עם גנים יפים.\n" +
                    "- כדאי להיכנס פנימה? כן, כדאי לבקר בגנים ובחצרות.\n" +
                    "- דמי כניסה? כן, לארמון ולמוזיאונים פנימיים.\n" +
                    "- זמן להישאר? 45-90 דקות.\n" +
                    "\n4. *Semperoper (בית האופרה)*\n" +
                    "- מרחק מארמון צווינגר: ~300 מטר (5 דקות הליכה או 2 דקות במונית).\n" +
                    "- מה יש שם? בית אופרה מהמם עם הופעות שונות.\n" +
                    "- כדאי להיכנס פנימה? כן, אם יש הופעה, אחרת תתפעל מבחוץ.\n" +
                    "- דמי כניסה? כן, להופעות.\n" +
                    "- זמן להישאר? 15-30 דקות.\n" +
                    "\n5. *המרפסת של ברוהל*\n" +
                    "- מרחק מ-Semperoper: ~500 מטר (6 דקות הליכה או 3 דקות במונית).\n" +
                    "- מה יש שם? שביל הליכה נופי עם נוף של נהר האלבה.\n" +
                    "- האם כדאי להיכנס פנימה? לא, זה אזור פתוח לטיול.\n" +
                    "- דמי כניסה? לא.\n" +
                    "- זמן להישאר? 30-60 דקות.\n" +
                    "\n6. *Residenzschloss (הארמון המלכותי)*\n" +
                    "- מרחק מהמרפסת של Brühl: ~400 מטר (5 דקות הליכה או 2 דקות במונית).\n" +
                    "- מה יש שם? ארמון מלכותי עם מוזיאונים ומשמעות היסטורית.\n" +
                    "- האם כדאי להיכנס פנימה? כן, שווה לחקור.\n" +
                    "- דמי כניסה? כן.\n" +
                    "- זמן להישאר? 60-90 דקות.\n" +
                    "\n7. *שלוס אלברכטסברג*\n" +
                    "- מרחק מ-Residenzschloss: ~2 ק\"מ (25 דקות הליכה או 8 דקות במונית).\n" +
                    "- מה יש שם? ארמון היסטורי על גבעה המשקיפה על נהר האלבה, עם גנים יפים.\n" +
                    "- האם כדאי להיכנס? כן, אם יש זמן, טיילו בגנים ובסביבה.\n" +
                    "- דמי כניסה? כן.\n" +
                    "- זמן להישאר? 60-90 דקות.\n" +
                    "\n8. *Fürstenzug (תהליך הנסיכים)*\n" +
                    "- מרחק משלוס אלברכטסברג: ~1.5 ק\"מ (20 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? ציור קיר פורצלן היסטורי המתאר את ההיסטוריה של משפחת המלוכה הסקסונית.\n" +
                    "- כדאי להיכנס פנימה? לא, זו יצירת אמנות חיצונית שאפשר להתפעל ממנה מבחוץ.\n" +
                    "- דמי כניסה? לא.\n" +
                    " - זמן להישאר? 10-15 דקות.\n" +
                    "\n9. *Kunsthofpassage*\n" +
                    "- מרחק מ-Fürstenzug: ~2.5 ק\"מ (30 דקות הליכה או 10 דקות במונית).\n" +
                    "- מה יש שם? אזור אמנותי וססגוני עם חנויות יצירתיות, גלריות, וואטer/ קירות בעיצוב מוזיקה.\n" +
                    "- כדאי להיכנס פנימה? כן, שווה לראות את האמנות הייחודית והמגוונת.\n" +
                    "- דמי כניסה? לא, אם כי חלק מהגלריות או החנויות עשויות לגבות.\n" +
                    "- זמן להישאר? 30-60 דקות.\n" +
                    "\n10. *Großer Garten*\n" +
                    "- מרחק מ-Kunsthofpassage: ~3 ק\"מ (35 דקות הליכה או 12 דקות במונית).\n" +
                    "- מה יש שם? פארק עצום עם שבילי הליכה/רכיבה על אופניים, גנים יפים ואטרקציות משפחתיות.\n" +
                    "- האם כדאי להיכנס פנימה? כן, תהנה מהפארק הפתוח ותרגע.\n" +
                    "- דמי כניסה? לא.\n" +
                    "- זמן להישאר? 60-120 דקות.\n" +
                    "\n11. *שוק חג המולד של דרזדן*\n" +
                    "- מרחק מ-Grosser Garten: ~3 ק\"מ (35 דקות הליכה או 12 דקות במונית).\n" +
                    "- מה יש שם? אחד משווקי חג המולד העתיקים בעולם במידה והגעתם בחורף, עם דוכנים המציעים אוכל מסורתי, מתנות ושתייה חמה.\n" +
                    " \"- כדאי להיכנס פנימה? כן, זה חובה לראות במהלך חג המולד.\n" +
                    "- דמי כניסה? לא.\n" +
                    "- זמן להישאר? 60-90 דקות.\n" +
                    "\n12. *Dresden Hbf (תחנת הרכבת המרכזית)*\n" +
                    "- מרחק משוק חג המולד של דרזדן: ~1 ק\"מ (12 דקות הליכה או 5 דקות במונית).\n" +
                    "- מה יש שם? מרכז התחבורה המרכזי של דרזדן.\n" +
                    "- האם כדאי להיכנס פנימה? כן, אם אתה ממשיך במסע שלך.\n" +
                    "- דמי כניסה? לא.\n" +
                    " \"- הגיע הזמן להישאר? רק אם אתה ממשיך במסע שלך.\n",  "https://www.omio.com/train-stations", "Link to purchase train tickets: "
            ));
        }

        if (days > 5) {
            ideas.add(new TripDetails(
                    "Day 6 - Vienna", "יום 6 - וינה",
                    "Vienna, the capital of Austria, is known for its imperial palaces, museums, and classical music history. It’s a city rich in culture and history.\n\n" +
                            "I recommend to take a train its a lot faster and cheaper than a bus. the Train station is near the Vaslev square. Tickets Below. "+
                            "Recommended places to visit in Vienna:\n" +
                            "1. *Schönbrunn Palace*\n" +
                            "- Distance from Vienna Hbf: ~20 minutes by metro.\n" +
                            "- What is there? A stunning palace and gardens.\n" +
                            "- Should you go inside? Yes, it’s one of the top attractions.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? Yes.\n\n" +
                            "2. *St. Stephen's Cathedral*\n" +
                            "- Distance from Schönbrunn: ~15 minutes by metro.\n" +
                            "- What is there? A magnificent Gothic cathedral.\n" +
                            "- Should you go inside? Yes, the interior is stunning.\n" +
                            "- How long to stay? 30-60 minutes.\n" +
                            "- Is there an entrance fee? Free to enter, but there’s a fee for the tower.\n\n" +
                            "3. *Hofburg Palace*\n" +
                            "- Distance from St. Stephen's Cathedral: ~5 minutes walking.\n" +
                            "- What is there? The former imperial palace, now home to museums and the Spanish Riding School.\n" +
                            "- Should you go inside? Yes, there are several museums inside.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? Yes.\n\n" +
                            "4. *Prater Park*\n" +
                            "- Distance from Hofburg: ~20 minutes by metro.\n" +
                            "- What is there? A large public park with a famous Ferris wheel.\n" +
                            "- Should you go inside? Yes, for a fun outdoor experience.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? Free to enter, but the Ferris wheel has a fee.","וינה, בירת אוסטריה, ידועה בארמונות האימפריאליים שלה, במוזיאונים ובהיסטוריה של המוזיקה הקלאסית. זוהי עיר עשירה בתרבות והיסטוריה.\n" +
                    "אני ממליץ לקחת רכבת זה הרבה יותר מהיר וזול מאוטובוס. תחנת הרכבת נמצאת ליד כיכר ואסלב. כרטיסים למטה. \n" +
                    "מקומות מומלצים לביקור בוינה:\n" +
                    "\n1. *ארמון שנברון*\n" +
                    "- מרחק מ-Vienna Hbf: ~20 דקות במטרו.\n" +
                    "- מה יש שם? ארמון וגנים מהממים.\n" +
                    "- האם כדאי להיכנס פנימה? כן, זו אחת האטרקציות המובילות.\n" +
                    "- כמה זמן להישאר? 1-2 שעות.\n" +
                    "- האם יש דמי כניסה? כן.\n" +
                    "\n2. *קתדרלת סטפן הקדוש*\n" +
                    "- מרחק משנברון: ~15 דקות במטרו.\n" +
                    "- מה יש שם? קתדרלה גותית מפוארת.\n" +
                    "- כדאי להיכנס פנימה? כן, הפנים מהמם.\n" +
                    "- כמה זמן להישאר? 30-60 דקות.\n" +
                    "- האם יש דמי כניסה? הכניסה חינם, אבל יש תשלום עבור המגדל.\n" +
                    "\n3. *ארמון הופבורג*\n" +
                    "- מרחק מקתדרלת סנט סטפן: ~5 דקות הליכה.\n" +
                    "- מה יש שם? הארמון הקיסרי לשעבר, כעת ביתם של מוזיאונים ובית הספר הספרדי לרכיבה.\n" +
                    "- האם כדאי להיכנס פנימה? כן, יש כמה מוזיאונים בפנים.\n" +
                    "- כמה זמן להישאר? 1-2 שעות.\n" +
                    "- האם יש דמי כניסה? כן.\n" +
                    "\n4. *פראטר פארק*\n" +
                    "- מרחק מהופבורג: ~20 דקות במטרו.\n" +
                    "- מה יש שם? פארק ציבורי גדול עם גלגל ענק מפורסם.\n" +
                    "- האם כדאי להיכנס פנימה? כן, לחוויה מהנה בחוץ.\n" +
                    " - כמה זמן להישאר? 1-2 שעות.\n" +
                    "- האם יש דמי כניסה? הכניסה חופשית, אבל הגלגל הענק בתשלום.\n","https://www.omio.com/train-stations", "Link to purchase train tickets: "
            ));
        }

        if (days > 6) {
            ideas.add(new TripDetails(
                    "Day 7 - Prague Jewish Quarter and River Cruise","יום 7 - הרובע היהודי ושייט בנהר",
                    "Prague’s Jewish Quarter is one of the best-preserved in Europe, and the Vltava River offers a great way to see the city.\n\n" +
                            "Recommended places to visit in Prague:\n" +
                            "1. *Jewish Quarter (Josefov)*\n" +
                            "- Distance from Old Town: ~10 minutes walking.\n" +
                            "- What is there? The historic Jewish quarter with synagogues and the Jewish Museum.\n" +
                            "- Should you go inside? Yes, it’s rich in history.\n" +
                            "- How long to stay? 1-2 hours.\n" +
                            "- Is there an entrance fee? Yes, for the museum and synagogues.\n\n" +
                            "2. *Charles Bridge*\n" +
                            "- Distance from Jewish Quarter: ~10 minutes walking.\n" +
                            "- What is there? A historic bridge over the Vltava River.\n" +
                            "- Should you go inside? No, it's an open bridge for a walk.\n" +
                            "- How long to stay? 30 minutes.\n"+
                            "- Is there an entrance fee? No.\n\n"+
                            "3. *Vltava River Cruise*\n" +
                            "- Distance from Charles Bridge: ~5 minutes walking.\n" +
                            "- What is there? A relaxing boat cruise along the river with views of Prague’s landmarks.\n" +
                            "- Should you go inside? Yes, it’s a lovely way to see the city.\n" +
                            "- How long to stay? 1 hour.\n" +
                            "- Is there an entrance fee? Yes, for the cruise.",
                   "הרובע היהודי של פראג הוא אחד השמורים ביותר באירופה, ונהר הוולטאבה מציע דרך מצוינת לראות את העיר.\n" +
                           "מקומות מומלצים לביקור בפראג:\n" +
                           "\n1. *הרובע היהודי (יוספוב)*\n" +
                           "- מרחק מהעיר העתיקה: ~10 דקות הליכה.\n" +
                           "- מה יש שם? הרובע היהודי ההיסטורי עם בתי כנסת והמוזיאון היהודי.\n" +
                           "- האם כדאי להיכנס פנימה? כן, זה עשיר בהיסטוריה.\n" +
                           "- כמה זמן להישאר? 1-2 שעות.\n" +
                           "- האם יש דמי כניסה? כן, למוזיאון ולבתי הכנסת.\n" +
                           "\n2. *גשר צ'ארלס*\n" +
                           "- מרחק מהרובע היהודי: ~10 דקות הליכה.\n" +
                           "- מה יש שם? גשר היסטורי מעל נהר הוולטאבה.\n" +
                           "- כדאי להיכנס פנימה? לא, זה גשר פתוח לטיול.\n" +
                           "- כמה זמן להישאר? 30 דקות.\n" +
                           "- האם יש דמי כניסה? לא.\n" +
                           "\n3. *שייט בנהר הוולטאבה*\n" +
                           "- מרחק מגשר קארל: ~5 דקות הליכה.\n" +
                           "- מה יש שם? שייט מרגיע בסירה לאורך הנהר עם נופים של ציוני הדרך של פראג.\n" +
                           "- האם כדאי להיכנס פנימה? כן, זו דרך מקסימה לראות את העיר.\n" +
                           "- כמה זמן להישאר? שעה אחת.\n" +
                           "- האם יש דמי כניסה? כן, לשייט.\n" , "https://www.tiqets.com/en/prague-river-cruises-l212165/?utm_source=google&utm_medium=cpc&utm_campaign=20969307723&utm_content=156829558206&gad_source=1&gclid=Cj0KCQiAy8K8BhCZARIsAKJ8sfR3cuIUALkOn97B4S06x7iVZwVdvqJosH4ppWIucn6rub3cY3c_Da4aAvmWEALw_wcB", "Link to buy the tickets to the cruise: "
            ));
        }
        return ideas;
    }
}
