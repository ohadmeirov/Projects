package com.example.MyTrip.fragments.countriesFragments;

import android.content.Context;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.inputmethod.InputMethodManager;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
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
 * Use the {@link HungaryFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class HungaryFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private RecyclerView recyclerViewHungary;

    public HungaryFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment HungaryFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static HungaryFragment newInstance(String param1, String param2) {
        HungaryFragment fragment = new HungaryFragment();
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
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_hungary, container, false);

        EditText editTextDays = view.findViewById(R.id.editTextDays);
        Button buttonShowTrips = view.findViewById(R.id.buttonShowTrips);
        recyclerViewHungary = view.findViewById(R.id.recyclerViewHungary);
        recyclerViewHungary.setLayoutManager(new LinearLayoutManager(getContext()));
        LinearLayout mainContainer = view.findViewById(R.id.mainContainer);
        ImageView imageView = view.findViewById(R.id.imageHungary);

        List<TripDetails> tripIdeas = new ArrayList<>();

        TripIdeasAdapter adapter = new TripIdeasAdapter(tripIdeas, mainContainer);
        recyclerViewHungary.setAdapter(adapter);

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

        Glide.with(this).asGif().load(R.drawable.hungarywaveflag).into(imageView);

        return view;
    }


    private List<TripDetails> generateTripIdeas(int days) {
        List<TripDetails> ideas = new ArrayList<>();

        ideas.add(new TripDetails(
                "Day 1: Exploring Budapest's Iconic Landmarks", "יום 1 - מגלים את הנקודות האייקוניות של בודפשט",
                "Hungarian Parliament Building (Address: Kossuth Lajos tér)\n" +
                        "Admire the stunning Neo-Gothic architecture and take a guided tour.\n" +
                        "St. Stephen’s Basilica (Address: V. Szent István tér 1)\n" +
                        "Visit the breathtaking basilica and climb to the dome for panoramic views of the city.\n" +
                        "Shoes on the Danube Bank\n" +
                        "A moving memorial to the victims of WWII along the Danube.\n" +
                        "House of Terror (Optional)\n" +
                        "Museum dedicated to Hungary's fascist and communist past.\n",
                "בניין הפרלמנט ההונגרי (כתובת: Kossuth Lajos tér)\n" +
                        "התפעל מהארכיטקטורה הניאו-גותית המדהימה וצא לסיור מודרך.\n" +
                        "בזיליקת סטפן הקדוש (כתובת: V. Szent István tér 1)\n" +
                        "בקרו בבזיליקה עוצרת הנשימה וטפסו אל הכיפה לנוף פנורמי של העיר.\n" +
                        " \"נעליים על גדת הדנובה\n" +
                        "אנדרטת זיכרון מרגשת לקורבנות מלחמת העולם השנייה לאורך הדנובה.\n" +
                        " \"בית הטרור (אופציונלי)\n" +
                        " מוזיאון המוקדש לעבר הפשיסטי והקומוניסטי של הונגריה.\n", "", ""
        ));

        if (days > 1) {
            ideas.add(new TripDetails(
                    "Day 2: Castle District and Fisherman’s Bastion","יום 2 - רובע הטירה ומבצר הדייגים",
                            "Fisherman’s Bastion\n" +
                                    "Enjoy the fairy-tale-like architecture and breathtaking views of the city.\n" +
                                    "Matthias Church (Address: Táncsics Mihály u. 26, 1014 Budapest)\n" +
                                    "Explore the stunning interiors of this historic church.\n" +
                                    "Buda Castle\n" +
                                    "Wander through the historic castle complex and visit the Hungarian National Gallery.\n" +
                                    "Citadella (Address: Citadella sétány 1, Gellért Hill)\n" +
                                    "End the day with a sunset view from Gellért Hill.\n"
                    ,"מבצר הדייגים\n" +
                    "תהנה מהארכיטקטורה דמוית האגדות ומהנוף עוצר הנשימה של העיר.\n" +
                    "Matthias Church (כתובת: Táncsics Mihály u. 26, 1014 Budapest)\n" +
                    "חקרו את הפנים המדהימים של הכנסייה ההיסטורית הזו.\n" +
                    "טירת בודה\n" +
                    "שוטט במתחם הטירה ההיסטורי ובקר בגלריה הלאומית של הונגריה.\n" +
                    "Citadella (כתובת: Citadella sétány 1, Gelért Hill)\n" +
                    " \"סיים את היום עם נוף שקיעה מגבעת גלרט.", "", ""
            ));
        }

        if (days > 2) {
            ideas.add(new TripDetails(
                    "Day 3: Cultural Highlights and Jewish Quarter","יום 3 - שיא תרבות והרובע היהודי",
                    "Great Market Hall (Address: Fővám tér)\n" +
                            "Shop for souvenirs, Hungarian specialties, and enjoy a light lunch.\n" +
                            "Dohány Street Synagogue (Address: Dohány utca 2-8)\n" +
                            "Visit Europe’s largest synagogue and the Holocaust Memorial.\n" +
                            "Jewish Quarter (Address: Kazinczy utca 14)\n" +
                            "Explore the ruin bars, street art, and history.\n" +
                            "Recommended: Grab a drink at Szimpla Kert.",
                    "יום 3 - שיא תרבות והרובע היהודי,\n" +
                            "היכל השוק הגדול (כתובת: Fővám tér)\n" +
                            "קנה מזכרות, התמחויות הונגריות, ותיהנה מארוחת צהריים קלה.\n" +
                            " \"Dohány Street Synagogue (כתובת: Dohány utca 2-8)\n" +
                            "בקר בבית הכנסת הגדול באירופה ובאנדרטת השואה.\n" +
                            "הרובע היהודי (כתובת: Kazinczy utca 14)\n" +
                            "חקור את הברים ההרוסים, אמנות הרחוב וההיסטוריה.\n"
                    ,"https://www.toureiffel.paris/en/rates-offers/ticket-second-floor-stairs?utm_source=magpietravelgttd", ""
            ));
        }

        if (days > 3) {
            ideas.add(new TripDetails(
                    "Day 4: Shopping Day","יום 4 - יום קניות",
                    "WestEnd City Center\n" +
                            "Modern shopping mall with international and local brands.\n" +
                            "Váci Street (Váci Utca)\n" +
                            "Stroll through this pedestrian shopping street for souvenirs and cafés.\n" +
                            "Arena Plaza\n" +
                            "Perfect spot for fashion, entertainment, and dining.",
                    "WestEnd City Center\n" +
                    "קניון מודרני עם מותגים בינלאומיים ומקומיים.\n" +
                    " \"Váci Street (Váci Utca)\n" +
                    "טייל במדרחוב הקניות הזה כדי למצוא מזכרות ובתי קפה.\n" +
                    "ארנה פלאזה\n" +
                    "מקום מושלם לאופנה, בידור וסעודה.\n","", ""
            ));
        }

        if (days > 4) {
            ideas.add(new TripDetails(
                    "Day 5 - Walking Tour of Budapest","יום 5 - וים סיור רגלי בבודפשט ",
                    "Andrássy Avenue (Andrássy út)\n" +
                            "Start your day with a stroll along this iconic boulevard, featuring beautiful architecture, luxury shops, and cozy cafés. Stop by the Hungarian State Opera House for a photo or quick tour.\n" +
                            "\n" +
                            "Heroes' Square (Hősök tere)\n" +
                            "At the end of Andrássy Avenue, visit this impressive square, a UNESCO World Heritage Site. Admire the statues of Hungarian leaders and the Millennium Monument.\n" +
                            "\n" +
                            "City Park (Városliget)\n" +
                            "Behind Heroes' Square, explore the peaceful park. Highlights include Vajdahunyad Castle and Széchenyi Thermal Bath, where you can enjoy the architecture and take a short break.\n" +
                            "\n" +
                            "Chain Bridge (Széchenyi Lánchíd)\n" +
                            "Walk across this iconic suspension bridge connecting Buda and Pest. Take in the panoramic views of the Danube and the city’s landmarks.\n" +
                            "\n" +
                            "Gellért Hill and Citadella\n" +
                            "Hike up to Gellért Hill for one of the best views of Budapest. Explore the Citadella and take in the cityscape as the sun begins to set.\n" +
                            "\n" +
                            "Váci Street (Váci Utca)\n" +
                            "End your day with a walk through this famous pedestrian street, perfect for shopping and dining. Grab a bite at one of the local restaurants or enjoy dessert at a café.",
                    "שדרת אנדראשי (Andrássy út)\n" +
                            "התחל את היום שלך בטיול לאורך השדרה האיקונית הזו, הכוללת ארכיטקטורה יפה, חנויות יוקרה ובתי קפה נעימים. עצרו ליד בית האופרה הממלכתי ההונגרי לצילום או סיור מהיר.\n" +
                            "\n" +
                            "כיכר הגיבורים (Hősök tere)\n" +
                            "בקצה שדרת אנדראשי, בקרו בכיכר המרשימה הזו, אתר מורשת עולמית של אונסק\"ו. התפעלו מהפסלים של המנהיגים ההונגרים ומאנדרטת המילניום.\n" +
                            "\n" +
                            "פארק העיר (Városliget)\n" +
                            "מאחורי כיכר הגיבורים, חקור את הפארק השליו. נקודות השיא כוללות את טירת Vajdahunyad ואת המרחצאות התרמיים Széchenyi, שם תוכלו ליהנות מהארכיטקטורה ולקחת הפסקה קצרה.\n" +
                            "\n" +
                            "גשר השלשלאות (Széchenyi Lánchíd)\n" +
                            "עבור על הגשר התלוי האייקוני הזה המחבר בין בודה לפשט. צפה בנוף הפנורמי של הדנובה ושל ציוני הדרך של העיר.\n" +
                            "\n" +
                            "Gellért Hill and Citadella\n" +
                            "צעד עד לגבעת גלרט לאחד מהנופים הטובים ביותר של בודפשט. חקור את המצודה וצפה בנוף העירוני כשהשמש מתחילה לשקוע.\n" +
                            "\n" +
                            "Váci Street (Váci Utca)\n" +
                            "סיים את היום שלך בהליכה במדרחוב המפורסם הזה, מושלם לקניות ולסעודה. תפוס ביס באחת המסעדות המקומיות או תיהנה מקינוח בבית קפה.","", ""
            ));
        }

        if (days > 5) {
            ideas.add(new TripDetails(
                    "Day 6 - Art and History","יום 6 - אומנות והסיטוריה",
                            "Hungarian National Museum (Address: VIII. Múzeum krt. 14-16)\n" +
                            "Dive into Hungary’s rich history and culture.\n" +
                            "Hungarian State Opera House (Address: Andrássy út 22)\n" +
                            "Take a guided tour or enjoy a performance.\n" +
                            "Heroes' Square and City Park\n",
                    "המוזיאון הלאומי ההונגרי (כתובת: VIII. Múzeum krt. 14-16)\n" +
                            "צלול לתוך ההיסטוריה והתרבות העשירה של הונגריה.\n" +
                            "בית האופרה הממלכתית של הונגריה (כתובת: Andrássy út 22)\n" +
                            "צא לסיור מודרך או תהנה מהופעה.\n" +
                            "כיכר הגיבורים ופארק העיר\n", "", ""
            ));
        }

        if (days > 6) {
            ideas.add(new TripDetails(
                    "Day 7: Day Trip to Szentendre\n","יום 7: טיול יום לסנטנדרה",
                    "Travel by HÉV train (M5) from Batthyány tér (45 minutes).\n" +
                            "Explore this charming artists’ town.\n" +
                            "Visit the Retro Museum, showcasing life during the communist era.\n" +
                            "Walk along the picturesque streets filled with galleries and boutiques.",
                    "נסיעה ברכבת HÉV (M5) מ-Batthyány tér (45 דקות).\n" +
                            "חקור את עיר האמנים המקסימה הזו.\n" +
                            "בקר במוזיאון הרטרו, מציג את החיים בעידן הקומוניסטי.\n" +
                            "לכו לאורך הרחובות הציוריים המלאים בגלריות ובבוטיקים.", "", ""
            ));
        }
        return ideas;
    }
}