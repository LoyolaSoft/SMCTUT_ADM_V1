using CMS.DAO;
using CMS.Models;
using CMS.SQL.Communication;
using CMS.SQL.Examination;
using CMS.SQL.Fee;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMS.Controllers
{
    public class FeeController : Controller
    {
        string sresult = string.Empty;
        int Number = 0;
        JsonResultData ObjJsonData = new JsonResultData();

        // GET: Fee

        //#region Testingfee

        //public ActionResult transferTest() {

        //    //string sIds = @"528,529,530,531,532,558,561,537,560,562,564,565,541,542,568,547,544,550,573,546,570,571,574,575,551,552,554,553,559,578,582,581,585,587,588,616,620,618,619,628,625,627,596,597,600,629,602,632,640,609,638,639,645,644,647,651,655,652,656,654,657,686,688,690,691,3,663,696,661,665,695,697,664,667,699,671,700,7,702,5,674,675,705,678,681,710,682,8,708,683,684,712,715,717,10,719,741,722,721,744,12,18,749,729,13,753,738,757,25,758,764,760,27,35,28,766,767,40,769,43,44,51,52,61,65,774,775,777,782,781,785,787,788,790,73,795,793,796,797,74,799,801,823,804,825,809,827,816,818,819,822,836,841,843,844,83,97,874,99,100,84,850,101,852,102,853,103,104,857,85,86,859,106,863,107,879,90,882,88,864,108,91,866,109,885,884,111,114,112,867,888,94,889,115,113,890,95,96,871,118,896,121,902,901,119,898,120,903,908,905,907,122,123,125,937,145,913,146,910,911,124,147,914,148,943,127,150,126,128,130,132,129,945,921,131,946,152,133,153,154,948,134,135,156,924,157,155,136,949,950,158,927,137,930,138,952,139,953,954,161,956,935,144,955,142,140,165,141,143,958,957,163,164,938,166,960,168,959,961,170,963,172,171,173,174,965,175,966,964,967,986,971,968,970,988,969,180,972,212,989,216,179,973,991,990,974,993,975,994,976,992,185,188,995,189,977,219,191,996,978,195,192,221,199,196,998,979,980,999,983,981,1000,202,227,984,206,985,228,231,1001,230,234,232,235,1002,237,238,1003,1006,240,241,242,243,1007,1008,1009,279,1010,245,1012,1011,248,247,249,1032,278,280,250,1016,1014,251,1015,253,282,252,1017,1018,285,284,1019,1033,258,1020,1021,260,1034,288,1023,1036,264,1035,263,267,290,1038,265,289,291,1025,268,269,292,1026,293,272,271,1027,294,1039,275,295,1029,296,276,299,298,301,300,1040,1041,302,303,306,307,308,309,311,1043,340,310,1044,345,313,316,347,352,348,315,1046,317,1055,318,351,353,319,1057,320,356,1047,1048,358,321,359,324,1051,323,1052,364,363,326,329,327,1058,367,1059,328,370,333,373,368,335,334,337,375,374,376,381,383,382,384,1064,385,413,412,391,388,414,390,392,415,416,393,395,398,396,1067,417,421,400,424,1068,422,1071,402,1069,403,404,406,425,408,410,409,1070,411,1075,1077,1076,429,1086,431,432,1081,1080,443,1089,1090,1088,435,444,1092,1083,438,1093,437,445,1085,1084,439,1094,448,447,449,442,450,1096,1095,452,455,456,457,471,1117,1118,1100,1102,458,475,476,1103,477,459,1122,1124,478,1125,1107,479,460,1126,480,463,1110,464,484,485,486,1112,467,487,465,488,1114,489,1113,1128,469,468,490,493,494,1145,1143,1146,1131,1149,510,511,1132,1133,1150,498,1151,497,515,499,517,1152,516,518,500,504,1154,1135,503,520,519,1137,521,522,507,1140,1139,506,523,508,509,1156,1158,1159,1176,1163,1180,1164,1165,1168,1182,1170,1185,1172,1174,1194,1198,1200,1204,1206,1209,1214,1216,1218,1217,1219,1224,1229,1230,1232,1538,1233,1541,1236,1547,1240,1544,1545,1551,1248,1250,1556,1555,1244,1553,1251,1252,1554,1558,1256,1257,1273,1560,1274,1275,1276,1263,1261,1278,1260,1282,1569,1568,1264,1561,1265,1267,1565,1563,1269,1571,1287,1289,1290,1566,1292,1293,1294,1573,1574,1575,1295,1298,1317,1301,1318,1319,1321,1583,1322,1577,1308,1304,1578,1307,1323,1324,1311,1312,1313,1579,1580,1328,1329,1330,1334,1335,1336,1337,1586,1587,1356,1588,1358,1339,1591,1341,1342,1360,1343,1344,1589,1345,1592,1362,1346,1347,1364,1365,1350,1368,1590,1367,1370,1371,1594,1373,1374,1375,1595,1389,1377,1391,1390,1392,1395,1380,1381,1383,1596,1397,1398,1384,1399,1385,1386,1401,1402,1403,1412,1413,1598,1414,1415,1407,1602,1408,1603,1417,1606,1421,1422,1423,1424,1609,1425,1426,1427,1428,1433,1611,1429,1430,1431,1435,1437,1439,1440,1615,1616,1442,1617,1443,1445,1446,1449,1450,1451,1448,1453,1452,1626,1455,1454,1622,1460,1628,1625,1458,1462,1463,1464,1459,1465,1466,1469,1470,1474,1475,1476,1486,1477,1487,1490,1491,1481,1482,1483,1636,1495,1496,1497,1499,1638,1637,1501,1642,1510,1643,1503,1512,1511,1504,1505,1644,1515,1518,1530,1521,1522,1524,1523,1526,1533,1535,1646,1647,1652,1653,1654,1661,1656,1655,1663,1664,1665,1667,1668,1669,1670,1671,1674,1672,1681,1673,1675,1684,1685,1677,1678,1689,1679,1691,1692,1694,1695,1700,1711,1713,1712,1717,1704,1719,1720,1724,1705,1726,1706,1707,1725,1727,1729,1708,1730,1733,1764,1760,1761,1762,1740,1763,1765,1742,1744,1746,1748,1745,1767,1769,1749,1768,1770,1771,1776,1755,1756,1777,1778,1757,1779,1782,1803,1784,1785,1786,1804,1806,1788,1787,1789,1809,1811,1790,1813,1792,1794,1796,1797,1805,1800,1801,1816,1818,1821,1822,1825,1829,1830,1832,1833,1839,1834,1836,1842,1841,1844,1843,1847,1846,1849,1837,1848,1850,1852,1851,1838,1853,1854,1855,1864,1865,1866,1856,1867,1857,1858,1859,1860,1861,1862,1863,1869,1870,1873,1872,1874,1876,2096,2097,1877,2099,2098,1879,2109,2101,2110,2104,2111,1880,2114,2115,2116,1882,2124,1885,1886,2125,2121,2118,2120,2127,2126,2130,2132,2133,2138,1891,2139,2134,2135,1894,2141,2137,2142,2143,1895,2144,2146,2147,2148,2149,2156,2152,2157,2158,2160,2162,2178,2180,2165,2181,2185,2184,2167,2168,2169,2186,2171,2174,2172,2175,2176,2188,2189,2190,2177,2191,2192,2195,2209,2210,2197,2212,2198,2199,2200,2213,2201,2214,2215,2203,2217,2221,2222,2239,1914,1916,2223,1915,2227,2240,2244,2225,2242,2228,1899,1898,2243,1917,1919,1900,1918,1920,1902,2246,2245,1921,2230,1901,1903,1922,1923,1924,2247,1904,1906,2249,1905,1926,2231,2229,1925,1927,2250,2232,1928,1907,1929,1930,2233,1931,2234,1909,1932,1908,1910,1912,1933,1911,1934,2236,1935,1913,2237,2252,2253,2254,2255,1936,2256,2257,2258,2259,1937,1938,2269,1951,2260,2270,2271,1939,1940,2272,2261,1941,1942,2273,2274,1943,2263,2262,1954,2264,2275,1953,1952,2276,1944,1955,1957,2265,1956,2277,1945,1946,1958,2278,1960,1959,1947,1961,2279,1963,1962,1964,2266,2267,1949,2268,1965,1950,1966,1968,2280,1967,2281,1969,2282,1970,1971,1972,1973,1985,2286,2283,1976,1978,1977,2287,1979,1987,1989,1980,1990,2288,2289,1981,1984,1982,1983,2290,2284,1992,2285,2291,1995,2293,2294,1996,1997,1999,1998,2000,2296,2006,2298,2002,2003,2301,2009,2305,2010,2302,2306,2308,2021,2012,2013,2014,2015,2016,2022,2311,2023,2017,2019,2020,2312,2025,2026,2027,2031,2032,2033,2317,2034,2316,2319,2037,2320,2029,2321,2322,2323,2043,2325,2044,2326,2327,2047,2053,2048,2329,2330,2331,2055,2054,2056,2049,2332,2337,2051,2333,2061,2338,2342,2060,2062,2345,2357,2063,2346,2347,2359,2066,2349,2361,2360,2352,2070,2073,2071,2354,2355,2363,2356,2075,2364,2366,2086,2367,2087,2076,2381,2368,2089,2370,2383,2384,2371,2372,2078,2090,2385,2091,2080,2374,2082,2083,2092,2386,2376,2387,2375,2084,2377,2378,2379,2393,2380,2395,2403,2405,2399,2401,2402,2406,2404,2410,2408,2409,2411,2412,2415,2746,2749,2747,2752,2495,2753,2750,2751,2755,2756,2498,2499,2496,2501,2760,2775,2776,2777,2507,2770,2771,2772,2779,2503,2508,2773,2780,2510,2774,2512,2527,2528,2793,2514,2795,2785,2798,2797,2530,2517,2788,2531,2532,2533,2789,2520,2790,2802,2803,2791,2535,2538,2537,2540,2541,2542,2818,2543,2552,2553,2544,2554,2555,2545,2556,2546,2557,2547,2814,2548,2815,2549,2550,2558,2551,2559,2560,2561,2562,2563,2564,2566,2565,2574,2567,2568,2570,2569,2575,2576,2824,2571,2826,2577,2572,2578,2573,2579,2580,2582,2581,2583,2584,2586,2590,2833,2591,2834,2835,2594,2838,2597,2839,2841,2600,2606,2602,2851,2850,2609,2603,2846,2847,2613,2849,2614,2615,2624,2859,2618,2620,2621,2622,2627,2856,2857,2623,2861,2628,2629,2630,2631,2636,2632,2868,2633,2870,2871,2634,2635,2638,2637,2878,2887,2639,2645,2644,2640,2648,2641,2888,2647,2649,2650,2651,2881,2653,2652,2882,2642,2643,2655,2654,2656,2891,2657,2658,2659,2660,2893,2900,2901,2663,2672,2664,2674,2665,2676,2904,2905,2681,2906,2909,2682,2907,2686,2687,2699,2700,2690,2701,2703,2705,2911,2695,2709,2716,2712,2913,2718,2914,2727,2918,2721,2722,2731,2916,2724,2917,2734,2735,2737,2739,2741,2919,2742,2920,2922,2743,2924,2744,2932,2934,2933,2937,2948,2938,2949,2940,2941,2943,2956,2957,2963,2958,2970,2961,2969,2972,2976,2974,2978,2980,2975,2982,2983,2984,2988,2991,2992,2993,2994,2997,2998,2999,3000,3004,3002,3006,3008,3011,3012,3016,3014,3013,3017,3018,3023,3019,3020,3021,3025,3026,3022,3027,3036,3030,3038,3040,3033,3047,3171,3054,3055,3050,3056,3059,3060,3051,3174,3176,3053,3063,3177,3065,3064,3179,3073,3074,3068,3069,3076,3077,3078,3180,3072,3182,3081,3183,3082,3185,3084,3186,3090,3184,3092,3097,3093,3094,3095,3098,3099,3100,3101,3188,3190,3189,3197,3192,3104,3195,3193,3200,3201,3202,3203,3214,3215,3207,3105,3216,3211,3209,3212,3218,3220,3223,3222,3227,3226,3237,3111,3106,3229,3230,3108,3232,3112,3234,3241,3240,3109,3113,3235,3236,3242,3244,3243,3115,3116,3247,3245,3246,3248,3123,3251,3118,3252,3124,3119,3125,3127,3128,3253,3254,3256,3121,3250,3257,3129,3130,3131,3258,3259,3265,3132,3267,3266,3268,3137,3133,3262,3263,3138,3270,3139,3134,3135,3136,3272,3141,3142,3273,3146,3144,3277,3280,3281,3282,3315,3318,3149,3312,3150,3320,3321,3322,3152,3326,3327,3332,3338,3153,3154,3347,3351,3352,3342,3354,3157,3359,3158,3159,3361,3362,3160,3365,3364,3378,3372,3161,3369,3371,3370,3381,3380,3382,3374,3375,3383,3384,3163,3164,3387,3165,3390,3166,3400,3403,3404,3407,3170,3405,3410,3413,3421,3667,3424,3425,3426,3670,3672,3675,3677,3679,3681,3682,3427,3430,3671,3432,3687,3433,3688,3434,3435,3691,3706,3437,3440,3441,3438,3692,3444,3442,3443,3708,3695,3693,3694,3445,3439,3697,3446,3447,3710,3449,3699,3448,3711,3712,3703,3450,3704,3452,3713,3454,3453,3715,3456,3457,3460,3458,3459,3733,3717,3461,3470,3732,3472,3734,3471,3473,3735,3462,3718,3719,3464,3474,3476,3475,3477,3738,3737,3722,3721,3740,3478,3741,3465,3742,3466,3467,3725,3479,3726,3480,3468,3481,3469,3727,3482,3728,3745,3730,3729,3744,3746,3487,3483,3485,3484,3488,3486,3489,3747,3490,3750,3751,3762,3491,3503,3752,3753,3492,3506,3754,3493,3494,3510,3495,3507,3496,3508,3509,3497,3514,3511,3512,3763,3513,3498,3515,3517,3519,3516,3499,3760,3518,3764,3500,3759,3761,3520,3766,3767,3521,3522,3768,3523,3524,3769,3537,3525,3539,3770,3526,3540,3773,3528,3527,3541,3774,3542,3775,3531,3530,3532,3533,3777,3543,3778,3779,3781,3780,3534,3535,3536,3544,3538,3545,3546,3783,3547,3548,3784,3551,3549,3550,3552,3785,3789,3786,3558,3791,3553,3556,3792,3555,3557,3788,3793,3794,3795,3559,3560,3797,3799,3803,3565,3800,3801,3566,3806,3807,3808,3809,3562,3802,3569,3568,3810,3575,3814,3811,3576,3815,3571,3817,3572,3573,3813,3578,3812,3579,3818,3581,3820,3586,3588,3823,3824,3830,3582,3825,3584,3591,3838,3836,3837,3855,3840,3839,3593,3842,3596,3858,3847,3848,3862,3853,3852,3864,3603,3868,3867,3610,3871,3615,3880,3872,3881,3882,3883,3620,3886,3894,3895,3890,3631,3629,3630,3891,3892,3896,3640,3642,3899,3644,3646,3654,3649,3656,3902,3651,3652,3657,3659,3660,3661,3662,3663,3664,3665,4411,";
        //    string sIds = @"12884,526,527,540,543,576,583,589,624,641,650,692,694,672,673,676,709,685,718,737,524,1774,3829,3826,3832,3833,3835,3841,3846,3849,3850,3851,3865,3866,3879,3869,3875,3885,3887,3888,3898,3900,6694,6690,4413,6703,4414,6704,4569,6705,6711,6713,6130,6131,6132,6133,6137,6161,6165,6167,6171,6146,6174,6147,6175,6730,6154,6182,6185,6728,6193,6197,6199,6202,6205,6725,6240,6206,6239,6211,6214,6213,6218,6245,6221,6220,6222,6223,6226,6229,6257,6234,6259,6268,6270,6288,6293,6735,6301,6275,6305,6281,6310,6309,6312,6316,6315,6319,6322,6323,6324,6736,6327,6326,6334,6333,6361,6336,6335,6337,6738,6368,6343,6345,6346,6350,6376,6378,6351,6357,6381,6382,6385,6386,6390,6411,6391,6392,6413,6414,6400,6416,6399,6420,6401,6424,6422,6406,6439,6443,6444,6445,6469,6468,6471,6453,6719,6504,6510,6490,6493,6496,6519,6521,6742,6522,6499,6501,6505,6556,6534,6536,6538,6542,6562,6569,6573,6576,6721,6584,6611,6613,6614,6616,6589,6592,6595,6597,6626,6628,6629,6633,6639,6645,6662,6647,6654,6656,6668,6680,6682,6674,6746,6699,6749,6751,6755,6756,6752,6758,6753,6764,6765,6767,6768,6769,6770,6772,6773,6784,6787,6792,6780,6782,6800,6803,6821,6830,6815,6811,6836,6837,6838,6840,6816,6854,6848,6851,6864,6904,6906,6910,6870,6871,6877,6875,6880,6922,6886,6921,6884,6894,6123,6129,6124,6939,6977,6979,6985,6989,6991,6950,6957,6954,6998,6956,7009,7042,7044,7046,7053,7023,7055,7026,7029,7033,7035,7063,7036,7065,7066,7067,7096,7097,7084,7086,7105,7090,7114,7113,7119,7094,7121,7125,7123,7126,7139,7141,7134,7133,7158,7160,7138,7175,7198,7200,7182,7209,7210,7186,7188,7213,7187,7216,7189,7217,7218,7220,7221,7222,7192,7194,7229,7232,7235,7255,7240,7258,7241,7243,7244,7245,7246,7248,7250,7263,7253,7266,7270,7279,7278,7273,7284,7292,7295,7393,7297,7298,7299,7300,7314,7302,7315,7317,7318,7319,7321,7323,7325,7324,7341,7328,7329,7343,7331,7332,7346,7347,7351,7355,7358,7359,7370,7368,7367,7377,7378,8041,8045,7398,7402,7401,7406,7403,8067,7412,7413,7416,7417,7426,7419,7433,7436,7423,7438,8138,7450,7490,8206,7466,7469,7498,7510,7476,7517,7520,7551,7528,7566,7569,7571,7540,7574,7575,7543,8026,7547,7589,7591,7593,7596,7601,7614,7617,7648,7620,7650,7653,7624,7660,7657,7662,7626,7664,7667,7669,7677,7715,7718,7684,7728,7730,7733,7693,7696,7738,7700,7737,7707,7712,7755,7778,7781,7780,7765,7786,7768,7770,7794,7793,7772,8504,7801,7831,7813,7839,7817,7848,7851,7850,7853,7855,7861,7889,7890,7875,7876,7899,7881,7882,7901,7903,7928,7938,7941,7944,7924,7949,7953,7962,7965,7967,7977,7969,7971,7974,7982,7983,7990,7993,7995,7996,8002,8005,8003,8004,8009,8012,8007,8017,8019,8020,8803,8934,9176,9250,9369,10208,10598,11185,";
        //    string[] strArray = sIds.Split(',');

        //    foreach (var item in strArray)
        //    {
        //        FeeRazorpayTransferAccountTesting(item, "1", "2019");
        //    }

        //return    View();
        //} 





        //private bool FeeRazorpayTransferAccountTesting(string sRazorpayId, string sFeeRootId, string sAcademicYear)
        //{
        //    bool bResult = false;
        //    try
        //    {
        //        List<FEE_RAZORPAY_PAYMENT_INFO> studentAcountList = new List<FEE_RAZORPAY_PAYMENT_INFO>();
        //        ArrayList lstTransfer = new ArrayList();
        //        Dictionary<string, object> options = new Dictionary<string, object>();
        //        string key = string.Empty;
        //        string secret = string.Empty;
        //        var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
        //        if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
        //        {
        //            key = FetchMarchantAccount.FirstOrDefault().KEY;
        //            secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
        //        }
        //        //if (sFeeRootId == Common.FeeRoot.CollegeFee)
        //            studentAcountList = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
        //        //else if (sFeeRootId == Common.FeeRoot.MessFee)
        //          //  studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //        // else if (sFeeRootId == Common.FeeRoot.HostelFee)
        //          //  studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //        // else if (sFeeRootId == Common.FeeRoot.HostelAndMess)
        //        // {
        //        // }
        //        //if (studentAcountList != null && studentAcountList.Count > 0)
        //        //{
        //            //foreach (var item in studentAcountList)
        //           // {
        //            //    var isTransferExisting = (List<FEE_RAZORPAY_TRANSFER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_TRANSFER_INFO>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeRazorpayTransferInfo), sAcademicYear).DataSource.Data;
        //              //  if (isTransferExisting == null)
        //               // {
        //                    ListDictionary listDictionary = new ListDictionary();
        //                    listDictionary.Add(Common.RazorpayKeyWords.AMOUNT, (int.Parse(studentAcountList.FirstOrDefault().AMOUNT)- int.Parse(studentAcountList.FirstOrDefault().FEE)).ToString());
        //                    listDictionary.Add(Common.RazorpayKeyWords.account, "acc_CkotmkPS9kakvr");
        //                    listDictionary.Add(Common.RazorpayKeyWords.CURRENCY, Common.RazorpayKeyWords.INR);
        //                    listDictionary.Add(Common.RazorpayKeyWords.NOTES, new Notes() );
        //                    lstTransfer.Add(listDictionary);
        //               // }
        //            //}

        //            options.Add(Common.RazorpayKeyWords.TRANSFERS, lstTransfer);
        //            RazorpayClient client = new RazorpayClient(key, secret);
        //            Payment payments = client.Payment.Fetch(studentAcountList.FirstOrDefault().ID);
        //            List<Transfer> transfer = payments.Transfer(options);
        //            //if (transfer != null && transfer.Count > 0)
        //            //{
        //            //    string sSQL = @"INSERT INTO `FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR`
        //            //                (
        //            //                `ID`,
        //            //                `ENTITY`,
        //            //                `SOURCE`,
        //            //                `RECIPIENT`,
        //            //                `AMOUNT`,
        //            //                `CURRENCY`,
        //            //                `AMOUNT_REVERSED`,
        //            //                `FEES`,
        //            //                `TAX`,
        //            //                `ON_HOLD`,
        //            //                `ON_HOLD_UNTIL`,
        //            //                `RECIPIENT_SETTLEMENT_ID`,
        //            //                `CREATED_AT`,
        //            //                `UDF1`,
        //            //                `UDF2`,
        //            //                `UDF3`,
        //            //                `UDF4`,
        //            //                `UDF5`,
        //            //                `UDF6`)
        //            //                VALUES ";
        //            //    foreach (var transferItem in transfer)
        //            //    {
        //            //        TransferItemResponse item = JsonConvert.DeserializeObject<TransferItemResponse>(transferItem.Attributes.ToString());
        //            //        sSQL += @"('" + item.id + "','" + item.entity + "','" + item.source + "','" + item.recipient + "','" + item.amount + "','" + item.currency + "'" +
        //            //            ",'" + item.amount_reversed + "','" + item.fees + "','" + item.tax + "','" + item.on_hold + "','" + item.on_hold_until + "'" +
        //            //            ",'" + item.recipient_settlement_id + "','" + item.created_at + "','" + item.notes.udf1 + "','" + item.notes.udf2 + "','" + item.notes.udf3 + "'" +
        //            //            ",'" + item.notes.udf4 + "','" + item.notes.udf5 + "','" + item.notes.udf6 + "'),";
        //            //    }
        //            //    sSQL = sSQL.TrimEnd(',');
        //            //    var result = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
        //            //    bResult = result.Success;
        //            //}

        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        using (ErrorLog objHandler = new ErrorLog())
        //        {
        //            objHandler.WriteError("Fee", "FeeRazorpayTransferAccount(string sRazorpayId, string sFeeRootId,string sAcademicYear)", "Err MSg: " + ex.Message, "Query is empty!");
        //            objHandler.WriteError("Fee", "FeeRazorpayTransferAccount(string sRazorpayId, string sFeeRootId,string sAcademicYear)", ex.Message);
        //        }

        //    }


        //    return bResult;
        //}
        //#endregion

        #region RazorPayActions



        public ActionResult ApplicationFeePayment(string sFrequencyTypeId, string sFeeRootId, string sProgrammeGroupId, string sUserId)
        {
            FeeTransactionViewModel liFeeTransactions = new FeeTransactionViewModel();
            OrderIdResponse objOrderInfo = new OrderIdResponse();
            Dictionary<string, object> input = new Dictionary<string, object>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Notes notes = new Notes();
            try
            {
                string key = string.Empty;
                string secret = string.Empty;
                List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {

                    //Validation .
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                    var issuedApplicationInfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString(), PROGRAMME_GROUP_ID = sProgrammeGroupId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchIssuedApplicationInfoByReceiveIdAndProgrammeGroupId), sAcademicYear).DataSource.Data;
                    if (issuedApplicationInfo != null)
                    {
                        Response.Write("<script>alert('You have applied already for this course!');</script>");
                        return RedirectToAction("DashboardIndex", "dashboard");
                    }
                    else
                    {

                        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = sFrequencyTypeId }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                        var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;

                        if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
                        {
                            key = FetchMarchantAccount.FirstOrDefault().KEY;
                            secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                        }

                        // 
                        string sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicatinInfo).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, FetchFrequency.FirstOrDefault().FREQUENCY_ID);
                        var applicationInfo = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(new FEE_STRUCTURE() { PROGRAMME = sProgrammeGroupId, CLASS_YEAR_ID = "1", RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, sSQL, sAcademicYear).DataSource.Data;

                        liFeeTransactions.liFeeStructure = applicationInfo;
                        if (applicationInfo.Sum(o => decimal.Parse(o.AMOUNT)) <= 0)
                        {
                            return RedirectToAction("DashboardIndex", "dashboard");
                        }
                        var FetchOrderInfo = (List<FEE_RAZORPAY_ORDER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_ORDER_INFO>(new FEE_RAZORPAY_ORDER_INFO()
                        {
                            UDF1 = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString(),
                            UDF2 = sFrequencyTypeId,
                            UDF3 = FetchFrequency.FirstOrDefault().FREQUENCY_ID,
                            UDF4 = sProgrammeGroupId,
                            UDF5 = sFeeRootId,
                            UDF6 = sProgrammeGroupId,
                            AMOUNT = applicationInfo.Sum(x => decimal.Parse(x.AMOUNT) * 100).ToString()
                        }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayOrderInfoByUDF), sAcademicYear).DataSource.Data;
                        if (FetchOrderInfo != null && FetchOrderInfo.Count > 0)
                        {
                            objOrderInfo.id = FetchOrderInfo.FirstOrDefault().ID;
                            objOrderInfo.amount = (decimal.Parse(FetchOrderInfo.FirstOrDefault().AMOUNT)).ToString();
                            objOrderInfo.notes.udf1 = FetchOrderInfo.FirstOrDefault().UDF1;
                            objOrderInfo.notes.udf2 = FetchOrderInfo.FirstOrDefault().UDF2;
                            objOrderInfo.notes.udf3 = FetchOrderInfo.FirstOrDefault().UDF3;
                            objOrderInfo.notes.udf4 = FetchOrderInfo.FirstOrDefault().UDF4;
                            objOrderInfo.notes.udf5 = FetchOrderInfo.FirstOrDefault().UDF5;
                            objOrderInfo.notes.udf6 = FetchOrderInfo.FirstOrDefault().UDF6;
                            liFeeTransactions.objOrderId = objOrderInfo;
                        }
                        else
                        {
                            notes.udf1 = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                            notes.udf2 = sFrequencyTypeId;
                            notes.udf3 = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                            notes.udf4 = sProgrammeGroupId;
                            notes.udf5 = sFeeRootId;
                            notes.udf6 = sProgrammeGroupId;
                            input.Add("amount", applicationInfo.Sum(x => decimal.Parse(x.AMOUNT) * 100)); // this amount should be same as transaction amount
                            input.Add("currency", "INR");
                            //  input.Add("receipt", "12121");
                            input.Add("payment_capture", 1);
                            input.Add("notes", notes);
                            RazorpayClient client = new RazorpayClient(key, secret);
                            var i = client.Order.Create(input).Attributes;
                            liFeeTransactions.objOrderId = JsonConvert.DeserializeObject<OrderIdResponse>(((Newtonsoft.Json.Linq.JToken)client.Order.Create(input).Attributes).Root.ToString());
                            if (liFeeTransactions.objOrderId != null)
                            {
                                var result = CMSHandler.SaveOrUpdate(new FEE_RAZORPAY_ORDER_INFO()
                                {
                                    AMOUNT = liFeeTransactions.objOrderId.amount,
                                    AMOUNT_DUE = liFeeTransactions.objOrderId.amount_due,
                                    AMOUNT_PAID = liFeeTransactions.objOrderId.amount_paid,
                                    ATTEMPTS = liFeeTransactions.objOrderId.attempts,
                                    CREATED_AT = liFeeTransactions.objOrderId.created_at,
                                    ENTITY = liFeeTransactions.objOrderId.entity,
                                    ID = liFeeTransactions.objOrderId.id,
                                    OFFER_ID = liFeeTransactions.objOrderId.offer_id,
                                    RECEIPT = liFeeTransactions.objOrderId.receipt,
                                    STATUS = liFeeTransactions.objOrderId.status,
                                    CURRENCY = liFeeTransactions.objOrderId.currency,
                                    UDF1 = liFeeTransactions.objOrderId.notes.udf1,
                                    UDF2 = liFeeTransactions.objOrderId.notes.udf2,
                                    UDF3 = liFeeTransactions.objOrderId.notes.udf3,
                                    UDF4 = liFeeTransactions.objOrderId.notes.udf4,
                                    UDF5 = liFeeTransactions.objOrderId.notes.udf5,
                                    UDF6 = liFeeTransactions.objOrderId.notes.udf6,

                                }, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveRazorpayOrderInfo), sAcademicYear);
                                if (result.Success)
                                {
                                    return View(liFeeTransactions);
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }

                        }
                        liFeeTransactions.objOrderId.key = key;
                        liFeeTransactions.objOrderId.secret = secret;
                    }

                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
                // return View(studentAcountList);

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "RazorPayPaymentGate(string sFrequencyTypeId, string sFeeRootId)", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "RazorPayPaymentGate(string sFrequencyTypeId, string sFeeRootId)", ex.Message);
                }
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(liFeeTransactions);

        }

        private bool ApplicationFeeResponse(string sRazorpayId, string sProgrammeGroupId, string sReceiveId, string sAcademicYear, string sFREQUENCY_ID)
        {
            bool bResult = false;
            string sStatus = string.Empty;
            var IsRegistered = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsRegistered)).DataSource.Data;
            if (IsRegistered != null && IsRegistered.Count > 0)
                sStatus = Common.STATUS.Submitted;
            else
                sStatus = Common.STATUS.Registered;
            var result = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { IS_PAID = "1", PAYMENT_MODE = Common.PaymentMode.Online, RAZORPAY_ID = sRazorpayId, RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeGroupId, STATUS = sStatus }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveIssueAppicationsAfterPayment), sAcademicYear);
            if (result.Success)
            {
                string sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectInsertsForApplicationFee).Replace(Common.Delimiter.QUS + Common.STUDENT_INFO.STUDENT_ID, sReceiveId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFREQUENCY_ID);
                var rs = CMSHandler.SaveOrUpdate(new FEE_STRUCTURE { FREQUENCY_ID = sFREQUENCY_ID, CLASS_YEAR_ID = "1", PROGRAMME = sProgrammeGroupId }, sSQL, sAcademicYear);
                //fetch application fee
                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectApplicationFee).Replace(Common.Delimiter.QUS + Common.STUDENT_INFO.STUDENT_ID, sReceiveId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFREQUENCY_ID);
                var amount = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(new FEE_STRUCTURE() { FREQUENCY_ID = sFREQUENCY_ID, CLASS_YEAR_ID = "1", PROGRAMME = sProgrammeGroupId }, sSQL, sAcademicYear).DataSource.Data;

                if (rs.Success)
                {
                    int total_amount = 0;
                    foreach (var item in amount)
                    {
                        total_amount = Convert.ToInt16(total_amount) + Convert.ToInt16(item.AMOUNT);
                    }
                    var issuedApplicationInfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeGroupId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchIssuedApplicationInfoByReceiveIdAndProgrammeGroupId), sAcademicYear).DataSource.Data;
                    if (issuedApplicationInfo != null && issuedApplicationInfo.Count > 0)
                    {
                        var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Application_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                        var sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issuedApplicationInfo.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, issuedApplicationInfo.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APP_FEE, total_amount.ToString());
                        var MessageContentTamil = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Application_FeeTamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                        var sContentTamil = MessageContentTamil.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issuedApplicationInfo.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, issuedApplicationInfo.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APP_FEE, total_amount.ToString());

                        SendEmailAndTextMessage(sContent, issuedApplicationInfo.FirstOrDefault().SMS_MOBILE_NO, issuedApplicationInfo.FirstOrDefault().EMAIL_ID, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Application No", "0");
                        SendEmailAndTextMessage(sContentTamil, issuedApplicationInfo.FirstOrDefault().SMS_MOBILE_NO, issuedApplicationInfo.FirstOrDefault().EMAIL_ID, MessageContentTamil.FirstOrDefault().SMS_TEMPLATE_ID, "Application No", "1");
                    }
                    bResult = true;
                }
            }
            return bResult;
        }
        private bool AdmissionFeeResponse(string sRazorpayId, string sProgrammeGroupId, string sReceiveId, string sAcademicYear, string sFREQUENCY_ID)
        {
            bool bResult = false;
            var result = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { IS_PAID = "1", PAYMENT_MODE = Common.PaymentMode.Online, RAZORPAY_ID = sRazorpayId, RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeGroupId, STATUS = Common.STATUS.Registered }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveIssueAppicationsAfterPayment), sAcademicYear);
            if (result.Success)
            {
                // var ser = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { IS_PAID = "1", PAYMENT_MODE = Common.PaymentMode.Online, RAZORPAY_ID = sRazorpayId, RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeGroupId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveIssueAppicationsAfterPayment), sAcademicYear);
                string sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectInsertsForApplicationFee).Replace(Common.Delimiter.QUS + Common.STUDENT_INFO.STUDENT_ID, sReceiveId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFREQUENCY_ID);
                var rs = CMSHandler.SaveOrUpdate(new FEE_STRUCTURE { FREQUENCY_ID = sFREQUENCY_ID, CLASS_YEAR_ID = "1", PROGRAMME = sProgrammeGroupId }, sSQL, sAcademicYear);
                if (rs.Success)
                {
                    var issuedApplicationInfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeGroupId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchIssuedApplicationInfoByReceiveIdAndProgrammeGroupId), sAcademicYear).DataSource.Data;
                    if (issuedApplicationInfo != null && issuedApplicationInfo.Count > 0)
                    {
                        var sContent = "Dear Parent, Applicant Name:" + issuedApplicationInfo.FirstOrDefault().FIRST_NAME + ",\n\nGreetings,\nPlease note your application no:" + issuedApplicationInfo.FirstOrDefault().APPLICATION_NO + ". \n \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                        SendEmailAndTextMessage(sContent, issuedApplicationInfo.FirstOrDefault().SMS_MOBILE_NO, issuedApplicationInfo.FirstOrDefault().EMAIL_ID, "Application No");
                    }

                    bResult = true;
                }
            }
            return bResult;
        }
        private void SendEmailAndTextMessage(string sContent, string sToNumber = "", string sToEmail = "", string sTemplateId = "", string sSubTitle = "", string IsTamil = "")
        {
            if (!string.IsNullOrEmpty(sContent))
            {
                List<MessageResult.sms> liSMS = new List<MessageResult.sms>();
                List<SMS_SETTING> liSMSSetting = new List<SMS_SETTING>();
                List<MessageResult.listsms> libulkSMS = new List<MessageResult.listsms>();
                string sFromEmail = ConfigurationManager.AppSettings["AdmissionFromMail"].ToString();
                string sPasswordEmail = ConfigurationManager.AppSettings["AdmissionPassword"].ToString();
                string sEmailOTPContent = string.Empty;
                string sEmailContent = string.Empty;
                //Fetch Active SMS Vendor
                var livendordetails = (List<SMS_VENDOR_DETAILS>)CMSHandler.FetchData<SMS_VENDOR_DETAILS>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchActiveSMSvendors)).DataSource.Data;
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_METHOD = Common.API_METHOD.API_1 }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchDovesoftSettings)).DataSource.Data;
                if (!string.IsNullOrEmpty(sToNumber))
                {
                    //---------------SMS------------
                    CommunicationController communication = new CommunicationController();
                    if (livendordetails != null && livendordetails.Count > 0)
                    {
                        liSMS.Add(new MessageResult.sms() { to = sToNumber, message = sContent, custom = "0", custom1 = "0", custom2 = "0" });
                        if (liSMSSetting != null && liSMSSetting.Count > 0)
                        {
                            libulkSMS.Add(new MessageResult.listsms() { mobiles = sToNumber, sms = sContent, senderid = liSMSSetting.FirstOrDefault().SENDER, clientSMSID = "1947692308", accountusagetypeid = "1", tempid = sTemplateId });
                        }
                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.Solutioninfini)
                        {
                            communication.sSmsMode = Common.MessageType.OTP;
                            int length = Convert.ToInt32(sContent.Length);

                            string sCount = CommonMethods.ValidateCreditCount(length);
                            if (sCount == "0")
                                sCount = "1";
                            communication.sCreditCount = sCount;
                            communication.liSMS = liSMS;
                            communication.sSmsCount = 1;
                            communication.SendBulkSMSFromSolutionInfi();
                        }
                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.DoveSoft)
                        {
                            communication.sTextMessage = sContent;
                            communication.sTemplateid = sTemplateId;
                            communication.sMobilenos = string.Join(",", liSMS.Select(o => o.to));
                            communication.SendBulkSMSFromDovesoft(IsTamil);
                        }
                    }

                }
                //-----------end sms--------------
                //-----------Email----------------   
                if (!string.IsNullOrEmpty(sToEmail))
                {
                    // CommonMethods.SendMailFromGmailSMTP(sFromEmail, sPasswordEmail, sToEmail, sSubTitle, sContent);
                }
                //----------Ending Email-------- -
            }

        }
        public JsonResultData UpdateSettlements()
        {
            JsonResultData oResult = new JsonResultData();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string sSQL = string.Empty;
                string sAcademicYear = string.Empty;
                string secret = string.Empty; string key = string.Empty;
                sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                List<FEE_RAZORPAY_TRANSFER_INFO> transfers = new List<FEE_RAZORPAY_TRANSFER_INFO>();
                transfers = (List<FEE_RAZORPAY_TRANSFER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_TRANSFER_INFO>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorPayTransfersForSettlemetUpdate), sAcademicYear).DataSource.Data;
                var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
                //Update OrderIds
                //UpdateOrderId(sAcademicYear);
                //UpdatePaymentsByOrderId(sAcademicYear);

                if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
                {
                    key = FetchMarchantAccount.FirstOrDefault().KEY;
                    secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                    if (transfers != null && transfers.Count > 0)
                    {

                        foreach (var item in transfers)
                        {
                            TransferItemResponse transferResponse = JsonConvert.DeserializeObject<TransferItemResponse>(CommonMethods.GetMethodForRazorpay("https://api.razorpay.com/v1/transfers/" + item.ID, key, secret));
                            if (transferResponse != null && !string.IsNullOrEmpty(transferResponse.recipient_settlement_id))
                            {
                                sSQL += "UPDATE FEE_RAZORPAY_TRANSFER_INFO_" + sAcademicYear + " SET RECIPIENT_SETTLEMENT_ID='" + transferResponse.recipient_settlement_id + "' WHERE RAZORPAY_TRANSFER_ID='" + item.RAZORPAY_TRANSFER_ID + "';";
                            }
                        }
                        using (MySQLHandler oHandler = new MySQLHandler())
                        {
                            oHandler.ExecuteAsScripts(sSQL);
                            oResult.ErrorCode = Common.ErrorCode.Ok;
                            oResult.Message = "Updated Successfully";
                        }
                    }
                }
                UpdateSettlementsInfo(sAcademicYear);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "UpdateSettlements", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "UpdateSettlements", ex.Message);
                }

            }

            return oResult;
        }

        public void UpdateSettlementsInfo(string sAcademicYear)
        {
            JsonResultData oResult = new JsonResultData();
            string sSQL = string.Empty;

            string secret = string.Empty; string key = string.Empty;
            sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            List<FEE_RAZORPAY_TRANSFER_INFO> transfers = new List<FEE_RAZORPAY_TRANSFER_INFO>();
            transfers = (List<FEE_RAZORPAY_TRANSFER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_TRANSFER_INFO>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorPaySettlementIds), sAcademicYear).DataSource.Data;
            var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;

            if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
            {
                key = FetchMarchantAccount.FirstOrDefault().KEY;
                secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                if (transfers != null && transfers.Count > 0)
                {
                    sSQL = @"INSERT INTO `FEE_RAZORPAY_SETTLEMENT_INFO_" + sAcademicYear + @"`
                            (
                            `ID`,
                            `ENTITY`,
                            `AMOUNT`,
                            `STATUS`,
                            `FEES`,
                            `TAX`,
                            `UTR`,
                            `CREATED_AT`,
                            `RECIPIENT`)
                            VALUES";
                    foreach (var item in transfers)
                    {
                        TransferResponse transferResponse = JsonConvert.DeserializeObject<TransferResponse>(CommonMethods.GetMethodForRazorpay("https://api.razorpay.com/v1/transfers?recipient_settlement_id=" + item.RECIPIENT_SETTLEMENT_ID + "&expand[]=recipient_settlement&count=1", key, secret));
                        if (transferResponse != null && transferResponse.count > 0)
                        {
                            var FetchSettlementInfo = (List<FEE_RAZORPAY_SETTLEMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_SETTLEMENT_INFO>(new FEE_RAZORPAY_SETTLEMENT_INFO() { ID = item.RECIPIENT_SETTLEMENT_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorPaySettlementInfoById), sAcademicYear).DataSource.Data;
                            if (FetchSettlementInfo == null)
                            {
                                sSQL += "('" + transferResponse.items.FirstOrDefault().recipient_settlement.id + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.entity + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.amount + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.status + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.fees + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.tax + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.utr + "','" + transferResponse.items.FirstOrDefault().recipient_settlement.created_at + "','" + transferResponse.items.FirstOrDefault().recipient + "'),";
                            }

                        }
                    }
                    sSQL = sSQL.TrimEnd(',');
                    using (MySQLHandler oHandler = new MySQLHandler())
                    {
                        oHandler.ExecuteAsScripts(sSQL);
                        oResult.ErrorCode = Common.ErrorCode.Ok;
                        oResult.Message = "Updated Successfully";
                    }
                }
            }
        }


        public void UpdateOrderId(string sAcademicYear)
        {
            JsonResultData oResult = new JsonResultData();
            string sSQL = string.Empty;

            string secret = string.Empty; string key = string.Empty;
            sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            List<FEE_RAZORPAY_ORDER_INFO> Orders = new List<FEE_RAZORPAY_ORDER_INFO>();
            Orders = (List<FEE_RAZORPAY_ORDER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_ORDER_INFO>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorPayOrderInfoForUpdate), sAcademicYear).DataSource.Data;
            var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;

            if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
            {
                key = FetchMarchantAccount.FirstOrDefault().KEY;
                secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                if (Orders != null && Orders.Count > 0)
                {
                    foreach (var item in Orders)
                    {
                        OrderIdResponse orderResponse = JsonConvert.DeserializeObject<OrderIdResponse>(CommonMethods.GetMethodForRazorpay("https://api.razorpay.com/v1/orders/" + item.ID, key, secret));
                        if (orderResponse != null)
                        {
                            if (orderResponse.status.ToLower() == "paid")
                                sSQL += "UPDATE FEE_RAZORPAY_ORDER_INFO_" + sAcademicYear + " SET STATUS='" + orderResponse.status + "' WHERE RAZORPAY_ORDER_ID='" + item.RAZORPAY_ORDER_ID + "';";
                        }
                    }
                    using (MySQLHandler oHandler = new MySQLHandler())
                    {
                        oHandler.ExecuteAsScripts(sSQL);
                        oResult.ErrorCode = Common.ErrorCode.Ok;
                        oResult.Message = "Updated Successfully";
                    }
                }
            }

        }

        public void UpdatePaymentsByOrderId(string sAcademicYear)
        {
            JsonResultData oResult = new JsonResultData();
            string sSQL = string.Empty;

            string secret = string.Empty; string key = string.Empty;
            sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            List<FEE_RAZORPAY_ORDER_INFO> Orders = new List<FEE_RAZORPAY_ORDER_INFO>();
            Orders = (List<FEE_RAZORPAY_ORDER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_ORDER_INFO>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorPayOrderInfoByPaidStatus), sAcademicYear).DataSource.Data;
            var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;

            if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
            {
                key = FetchMarchantAccount.FirstOrDefault().KEY;
                secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                if (Orders != null && Orders.Count > 0)
                {
                    foreach (var item in Orders)
                    {
                        TransferResponse orderResponse = JsonConvert.DeserializeObject<TransferResponse>(CommonMethods.GetMethodForRazorpay("https://api.razorpay.com/v1/payments?order_id=" + item.ID, key, secret));
                        if (orderResponse != null)
                        {
                            foreach (var innerItem in orderResponse.items)
                            {
                                if (innerItem.captured == true)
                                {
                                    RazorPayPaymentResponseEntryBySystem(innerItem.id);
                                }
                            }
                        }
                    }
                }
            }

        }
        public ActionResult RazorPayPaymentGate(string sFrequencyTypeId, string sFeeRootId)
        {

            FeeTransactionViewModel liFeeTransactions = new FeeTransactionViewModel();
            OrderIdResponse objOrderInfo = new OrderIdResponse();
            Dictionary<string, object> input = new Dictionary<string, object>();
            Notes notes = new Notes();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                string key = string.Empty;
                string secret = string.Empty;
                List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = sFrequencyTypeId }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;

                    if (FetchMarchantAccount != null & FetchMarchantAccount.Count() > 0)
                    {
                        key = FetchMarchantAccount.FirstOrDefault().KEY;
                        secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                    }

                    if (sFrequencyTypeId == Common.FrequencyType.SemesterFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.SemesterFee, FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.MessFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.MessFee, FEE_ROOT_ID = Common.FeeRoot.MessFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FecthMessFeesByStudentIdAndFeeRootId), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.HostelFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.HostelFee, FEE_ROOT_ID = Common.FeeRoot.HostelFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FecthMessFeesByStudentIdAndFeeRootId), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.ExamFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.ExamFee, FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.AdmissionFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.AdmissionFee, FEE_ROOT_ID = Common.FeeRoot.CollegeFee , PROGRAMME_MODE = "1" }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoSMC), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.LibraryFee)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.LibraryFee, FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.HostelApplication)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.HostelApplication, FEE_ROOT_ID = Common.FeeRoot.HostelApplicationfee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountHostelFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    else if (sFrequencyTypeId == Common.FrequencyType.AdmissionFeeSSC)
                        studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = Common.FrequencyType.AdmissionFeeSSC, FEE_ROOT_ID = Common.FeeRoot.CollegeFee ,PROGRAMME_MODE = "2"}, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoSMC), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;

                    liFeeTransactions.liFeeStudentAccount = studentAcountList;
                    if (studentAcountList.Sum(o => decimal.Parse(o.BALANCE)) <= 0)
                    {
                        return RedirectToAction("DashboardIndex", "dashboard");
                    }
                    var FetchOrderInfo = (List<FEE_RAZORPAY_ORDER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_ORDER_INFO>(new FEE_RAZORPAY_ORDER_INFO()
                    {
                        UDF1 = Session[Common.SESSION_VARIABLES.USER_ID].ToString(),
                        UDF2 = sFrequencyTypeId,
                        UDF5 = sFeeRootId,
                        UDF4 = studentAcountList.FirstOrDefault().PROGRAMME_ID,
                        UDF3 = FetchFrequency.FirstOrDefault().FREQUENCY_ID,
                        UDF6 = studentAcountList.FirstOrDefault().PROGRAMME_ID,
                        AMOUNT = studentAcountList.Sum(x => decimal.Parse(x.BALANCE) * 100).ToString()
                    }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayOrderInfoByUDF), sAcademicYear).DataSource.Data;
                    if (FetchOrderInfo != null && FetchOrderInfo.Count > 0)
                    {
                        objOrderInfo.id = FetchOrderInfo.FirstOrDefault().ID;
                        objOrderInfo.amount = (decimal.Parse(FetchOrderInfo.FirstOrDefault().AMOUNT)).ToString();
                        objOrderInfo.notes.udf1 = FetchOrderInfo.FirstOrDefault().UDF1;
                        objOrderInfo.notes.udf2 = FetchOrderInfo.FirstOrDefault().UDF2;
                        objOrderInfo.notes.udf3 = FetchOrderInfo.FirstOrDefault().UDF3;
                        objOrderInfo.notes.udf4 = FetchOrderInfo.FirstOrDefault().UDF4;
                        objOrderInfo.notes.udf5 = FetchOrderInfo.FirstOrDefault().UDF5;
                        objOrderInfo.notes.udf6 = FetchOrderInfo.FirstOrDefault().UDF6;
                        liFeeTransactions.objOrderId = objOrderInfo;
                    }
                    else
                    {
                        notes.udf1 = studentAcountList.FirstOrDefault().STUDENT_ID;
                        notes.udf2 = sFrequencyTypeId;
                        notes.udf3 = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        notes.udf4 = studentAcountList.FirstOrDefault().PROGRAMME_ID;
                        notes.udf5 = sFeeRootId;
                        notes.udf6 = studentAcountList.FirstOrDefault().PROGRAMME_ID;
                        input.Add("amount", studentAcountList.Sum(x => decimal.Parse(x.BALANCE) * 100)); // this amount should be same as transaction amount
                        input.Add("currency", "INR");
                        //input.Add("receipt", "12121");
                        input.Add("payment_capture", 1);
                        input.Add("notes", notes);
                        RazorpayClient client = new RazorpayClient(key, secret);
                        var i = client.Order.Create(input).Attributes;
                        liFeeTransactions.objOrderId = JsonConvert.DeserializeObject<OrderIdResponse>(((Newtonsoft.Json.Linq.JToken)client.Order.Create(input).Attributes).Root.ToString());
                        if (liFeeTransactions.objOrderId != null)
                        {
                            var result = CMSHandler.SaveOrUpdate(new FEE_RAZORPAY_ORDER_INFO()
                            {
                                AMOUNT = liFeeTransactions.objOrderId.amount,
                                AMOUNT_DUE = liFeeTransactions.objOrderId.amount_due,
                                AMOUNT_PAID = liFeeTransactions.objOrderId.amount_paid,
                                ATTEMPTS = liFeeTransactions.objOrderId.attempts,
                                CREATED_AT = liFeeTransactions.objOrderId.created_at,
                                ENTITY = liFeeTransactions.objOrderId.entity,
                                ID = liFeeTransactions.objOrderId.id,
                                OFFER_ID = liFeeTransactions.objOrderId.offer_id,
                                RECEIPT = liFeeTransactions.objOrderId.receipt,
                                STATUS = liFeeTransactions.objOrderId.status,
                                CURRENCY = liFeeTransactions.objOrderId.currency,
                                UDF1 = liFeeTransactions.objOrderId.notes.udf1,
                                UDF2 = liFeeTransactions.objOrderId.notes.udf2,
                                UDF3 = liFeeTransactions.objOrderId.notes.udf3,
                                UDF4 = liFeeTransactions.objOrderId.notes.udf4,
                                UDF5 = liFeeTransactions.objOrderId.notes.udf5,
                                UDF6 = liFeeTransactions.objOrderId.notes.udf6,

                            }, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveRazorpayOrderInfo), sAcademicYear);
                            if (result.Success)
                            {
                                return View(liFeeTransactions);
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                    liFeeTransactions.objOrderId.key = key;
                    liFeeTransactions.objOrderId.secret = secret;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
                // return View(studentAcountList);

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "RazorPayPaymentGate(string sFrequencyTypeId, string sFeeRootId)", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "RazorPayPaymentGate(string sFrequencyTypeId, string sFeeRootId)", ex.Message);
                }
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(liFeeTransactions);
        }
        // this method is response point for  pay
        public ActionResult RazorPayPaymentResponse(string PaymentId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            FeeTransactionViewModel liFeeTransactions = new FeeTransactionViewModel();
            OrderIdResponse objOrderInfo = new OrderIdResponse();
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
            Dictionary<string, object> input = new Dictionary<string, object>();
            Notes notes = new Notes();
            string key = string.Empty;
            string secret = string.Empty;
            string sRazorpayPaymentId = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                    var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
                    if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
                    {
                        key = FetchMarchantAccount.FirstOrDefault().KEY;
                        secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                    }
                    RazorpayClient client = new RazorpayClient(key, secret);
                    Payment paymentResponse = client.Payment.Fetch(PaymentId);
                    PaymentResponse objPaymentInfo = new PaymentResponse();
                    objPaymentInfo = JsonConvert.DeserializeObject<PaymentResponse>(((Newtonsoft.Json.Linq.JToken)paymentResponse.Attributes).Root.ToString());
                    var o = (FEE_RAZORPAY_PAYMENT_INFO)CommonMethods.PropertyMapping<PaymentResponse, FEE_RAZORPAY_PAYMENT_INFO>(objPaymentInfo, null);

                    o.UDF1 = objPaymentInfo.notes.udf1;
                    o.UDF2 = objPaymentInfo.notes.udf2;
                    o.UDF3 = objPaymentInfo.notes.udf3;
                    o.UDF4 = objPaymentInfo.notes.udf4;
                    o.UDF5 = objPaymentInfo.notes.udf5;
                    o.UDF6 = objPaymentInfo.notes.udf6;
                    if (string.IsNullOrEmpty(objPaymentInfo.notes.udf6))
                    {
                        o.UDF6 = objPaymentInfo.notes.udf4;
                    }
                    var isExisting = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = PaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
                    if (isExisting != null && isExisting.Count > 0)
                    {
                        return View(studentAcountList);
                    }
                    var restult = CMSHandler.SaveOrUpdate(o, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveRazorpayPaymentInfo), sAcademicYear);

                    if (restult.Success)
                    {
                        var FetchRazorpayPaymentInfo = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = o.ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
                        //Creating application no and student_account.
                        if (FetchRazorpayPaymentInfo.FirstOrDefault().UDF2 == Common.FrequencyType.AdmissionApplicationFee)
                        {
                            var IssueApplicationStatus = ApplicationFeeResponse(FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID, FetchRazorpayPaymentInfo.FirstOrDefault().UDF6, FetchRazorpayPaymentInfo.FirstOrDefault().UDF1, sAcademicYear, FetchRazorpayPaymentInfo.FirstOrDefault().UDF3);
                            if (!IssueApplicationStatus)
                            {
                                return RedirectToAction("ErrorMessage", "Error");
                            }
                        }
                        sRazorpayPaymentId = (FetchRazorpayPaymentInfo != null && FetchRazorpayPaymentInfo.Count > 0) ? FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID : string.Empty;
                        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = objPaymentInfo.notes.udf2 }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                        var result = FeeRazorpayCollection(o, sRazorpayPaymentId, sAcademicYear);

                        if (!string.IsNullOrEmpty(result.RECEIPT_NO))
                        {
                            FeeRazorpayTransferAccount(sRazorpayPaymentId, o.UDF5, sAcademicYear);

                            if (o.UDF2 == Common.FrequencyType.AdmissionApplicationFee)
                            {
                                studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                            }
                            else if (o.UDF5 == Common.FeeRoot.CollegeFee)
                            {
                                //Have to send the message for college Admission    
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = o.UDF1, STATUS = Common.STATUS.Admitted, PROGRAMME_ID = o.UDF4 }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = o.UDF1, PROGRAMME_ID = o.UDF4 }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                var MessageContents = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Admission_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                if (MessageContents != null && MessageContents.Count > 0)
                                {
                                    if (StudentAccount != null && StudentAccount.Count > 0)
                                    {
                                        string Content = MessageContents.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.txnid, PaymentId).Replace(Common.Delimiter.AT + Common.amount, studentAcountList.Sum(s => Convert.ToInt32(s.BALANCE)).ToString());
                                        SendEmailAndTextMessage(Content, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, MessageContents.FirstOrDefault().SMS_TEMPLATE_ID, "  Admission Fee Status From St. Mary's College, Thoothukudi.");
                                    }
                                }
                            }
                            else if (o.UDF5 == Common.FeeRoot.HostelFee)
                            {
                                //Have to send the message for Hostel Admission    
                                studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                var HostelStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = o.UDF1, STATUS = Common.STATUS.Admitted, PROGRAMME_ID = o.UDF4 }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveIdInHostelselection), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = o.UDF1, PROGRAMME_ID = o.UDF4 }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                var MessageContents = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Admission_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                if (MessageContents != null && MessageContents.Count > 0)
                                {
                                    if (StudentAccount != null && StudentAccount.Count > 0)
                                    {
                                        string Content = MessageContents.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.txnid, PaymentId).Replace(Common.Delimiter.AT + Common.amount, studentAcountList.Sum(s => Convert.ToInt32(s.BALANCE)).ToString());
                                        SendEmailAndTextMessage(Content, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, " Hostel  Admission Fee Status From St. Mary's College, Thoothukudi.");
                                    }
                                }
                            }
                            else if (o.UDF5 == Common.FeeRoot.HostelApplicationfee)
                            {
                                studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentHostelApplicationFeeStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                            }
                            else if (o.UDF5 == Common.FeeRoot.HostelAndMess)
                            {
                            }
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "RazorPayPaymentResponse(string PaymentId)", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "RazorPayPaymentResponse(string PaymentId)", ex.Message);
                    }
                }


            }
            else
                return RedirectToAction("ErrorMessage", "Error");

            return View(studentAcountList);
        }
        public void RazorPayPaymentResponseEntryBySystem(string PaymentId)
        {
            FeeTransactionViewModel liFeeTransactions = new FeeTransactionViewModel();
            OrderIdResponse objOrderInfo = new OrderIdResponse();
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
            Dictionary<string, object> input = new Dictionary<string, object>();
            Notes notes = new Notes();
            string key = string.Empty;
            string secret = string.Empty;
            string sRazorpayPaymentId = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                    var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
                    if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
                    {
                        key = FetchMarchantAccount.FirstOrDefault().KEY;
                        secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                    }
                    RazorpayClient client = new RazorpayClient(key, secret);
                    Payment paymentResponse = client.Payment.Fetch(PaymentId);
                    PaymentResponse objPaymentInfo = new PaymentResponse();
                    objPaymentInfo = JsonConvert.DeserializeObject<PaymentResponse>(((Newtonsoft.Json.Linq.JToken)paymentResponse.Attributes).Root.ToString());
                    var o = (FEE_RAZORPAY_PAYMENT_INFO)CommonMethods.PropertyMapping<PaymentResponse, FEE_RAZORPAY_PAYMENT_INFO>(objPaymentInfo, null);

                    o.UDF1 = objPaymentInfo.notes.udf1;
                    o.UDF2 = objPaymentInfo.notes.udf2;
                    o.UDF3 = objPaymentInfo.notes.udf3;
                    o.UDF4 = objPaymentInfo.notes.udf4;
                    o.UDF5 = objPaymentInfo.notes.udf5;
                    o.UDF6 = objPaymentInfo.notes.udf6;
                    var isExisting = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = PaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
                    if (isExisting == null)
                    {
                        var restult = CMSHandler.SaveOrUpdate(o, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveRazorpayPaymentInfo), sAcademicYear);
                        if (restult.Success)
                        {
                            var FetchRazorpayPaymentInfo = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = o.ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
                            sRazorpayPaymentId = (FetchRazorpayPaymentInfo != null && FetchRazorpayPaymentInfo.Count > 0) ? FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID : string.Empty;
                            //Creating application no and student_account.
                            if (FetchRazorpayPaymentInfo.FirstOrDefault().UDF2 == Common.FrequencyType.AdmissionApplicationFee)
                            {
                                var IssueApplicationStatus = ApplicationFeeResponse(FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID, FetchRazorpayPaymentInfo.FirstOrDefault().UDF6, FetchRazorpayPaymentInfo.FirstOrDefault().UDF1, sAcademicYear, FetchRazorpayPaymentInfo.FirstOrDefault().UDF3);
                                if (!IssueApplicationStatus)
                                {
                                    return;
                                }
                            }
                            var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = objPaymentInfo.notes.udf2 }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                            var result = FeeRazorpayCollection(o, sRazorpayPaymentId, sAcademicYear);

                            if (!string.IsNullOrEmpty(result.RECEIPT_NO))
                            {
                                FeeRazorpayTransferAccount(sRazorpayPaymentId, o.UDF5, sAcademicYear);
                                if (o.UDF5 == Common.FeeRoot.CollegeFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.MessFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.HostelFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.HostelAndMess)
                                {
                                }
                            }
                        }
                        else
                        {
                            var FetchRazorpayPaymentInfo = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(new FEE_RAZORPAY_PAYMENT_INFO() { ID = o.ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentInfoById), sAcademicYear).DataSource.Data;
                            sRazorpayPaymentId = (FetchRazorpayPaymentInfo != null && FetchRazorpayPaymentInfo.Count > 0) ? FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID : string.Empty;
                            //Creating application no and student_account.
                            if (FetchRazorpayPaymentInfo.FirstOrDefault().UDF2 == Common.FrequencyType.AdmissionApplicationFee)
                            {
                                var IssueApplicationStatus = ApplicationFeeResponse(FetchRazorpayPaymentInfo.FirstOrDefault().RAZORPAY_PAMENT_ID, FetchRazorpayPaymentInfo.FirstOrDefault().UDF6, FetchRazorpayPaymentInfo.FirstOrDefault().UDF1, sAcademicYear, FetchRazorpayPaymentInfo.FirstOrDefault().UDF3);
                                //if (!IssueApplicationStatus)
                                //{
                                //    return;
                                //}
                            }
                            var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = objPaymentInfo.notes.udf2 }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                            var result = FeeRazorpayCollection(o, sRazorpayPaymentId, sAcademicYear);

                            if (!string.IsNullOrEmpty(result.RECEIPT_NO))
                            {
                                FeeRazorpayTransferAccount(sRazorpayPaymentId, o.UDF5, sAcademicYear);
                                if (o.UDF5 == Common.FeeRoot.CollegeFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.MessFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.HostelFee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.HostelApplicationfee)
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                else if (o.UDF5 == Common.FeeRoot.HostelAndMess)
                                {
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "RazorPayPaymentResponseEntryBySystem(string PaymentId)", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "RazorPayPaymentResponseEntryBySystem(string PaymentId)", ex.Message);
                    }
                }


            }
            else { }
            // return RedirectToAction("ErrorMessage", "Error");

            //return View(studentAcountList);
        }
        //this method is make route transactions
        private bool FeeRazorpayTransferAccount(string sRazorpayId, string sFeeRootId, string sAcademicYear)
        {
            bool bResult = false;
            try
            {
                List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
                ArrayList lstTransfer = new ArrayList();
                Dictionary<string, object> options = new Dictionary<string, object>();
                string key = string.Empty;
                string secret = string.Empty;
                var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
                if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
                {
                    key = FetchMarchantAccount.FirstOrDefault().KEY;
                    secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                }
                if (sFeeRootId == Common.FeeRoot.CollegeFee)
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                else if (sFeeRootId == Common.FeeRoot.MessFee)
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                else if (sFeeRootId == Common.FeeRoot.HostelFee)
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                else if (sFeeRootId == Common.FeeRoot.HostelAndMess)
                {
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;

                }
                else if (sFeeRootId == Common.FeeRoot.HostelApplicationfee)
                {
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sRazorpayId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                }
                if (studentAcountList != null && studentAcountList.Count > 0)
                {
                    foreach (var item in studentAcountList)
                    {
                        var isTransferExisting = (List<FEE_RAZORPAY_TRANSFER_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_TRANSFER_INFO>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeRazorpayTransferInfo), sAcademicYear).DataSource.Data;
                        if (isTransferExisting == null)
                        {
                            ListDictionary listDictionary = new ListDictionary();
                            listDictionary.Add(Common.RazorpayKeyWords.AMOUNT, (decimal.Parse(item.BALANCE) * 100).ToString());
                            listDictionary.Add(Common.RazorpayKeyWords.account, item.ACCOUNT_ID);
                            listDictionary.Add(Common.RazorpayKeyWords.CURRENCY, Common.RazorpayKeyWords.INR);
                            listDictionary.Add(Common.RazorpayKeyWords.NOTES, new Notes() { udf1 = item.STUDENT_ID, udf2 = item.RAZORPAY_ID, udf3 = item.MAIN_HEAD_ID, udf4 = item.BANK_ACCOUNT_ID });
                            lstTransfer.Add(listDictionary);
                        }
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    options.Add(Common.RazorpayKeyWords.TRANSFERS, lstTransfer);
                    RazorpayClient client = new RazorpayClient(key, secret);
                    Payment payments = client.Payment.Fetch(studentAcountList.FirstOrDefault().ID);
                    List<Transfer> transfer = payments.Transfer(options);
                    if (transfer != null && transfer.Count > 0)
                    {
                        string sSQL = @"INSERT INTO `FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR`
                                    (
                                    `ID`,
                                    `ENTITY`,
                                    `SOURCE`,
                                    `RECIPIENT`,
                                    `AMOUNT`,
                                    `CURRENCY`,
                                    `AMOUNT_REVERSED`,
                                    `FEES`,
                                    `TAX`,
                                    `ON_HOLD`,
                                    `ON_HOLD_UNTIL`,
                                    `RECIPIENT_SETTLEMENT_ID`,
                                    `CREATED_AT`,
                                    `UDF1`,
                                    `UDF2`,
                                    `UDF3`,
                                    `UDF4`,
                                    `UDF5`,
                                    `UDF6`)
                                    VALUES ";
                        foreach (var transferItem in transfer)
                        {
                            TransferItemResponse item = JsonConvert.DeserializeObject<TransferItemResponse>(transferItem.Attributes.ToString());
                            sSQL += @"('" + item.id + "','" + item.entity + "','" + item.source + "','" + item.recipient + "','" + item.amount + "','" + item.currency + "'" +
                                ",'" + item.amount_reversed + "','" + (!string.IsNullOrEmpty(item.fees) ? item.fees : "0") + "','" + (!string.IsNullOrEmpty(item.tax) ? item.tax : "0") + "','" + item.on_hold + "','" + item.on_hold_until + "'" +
                                ",'" + item.recipient_settlement_id + "','" + item.created_at + "','" + item.notes.udf1 + "','" + item.notes.udf2 + "','" + item.notes.udf3 + "'" +
                                ",'" + item.notes.udf4 + "','" + item.notes.udf5 + "','" + item.notes.udf6 + "'),";
                        }
                        sSQL = sSQL.TrimEnd(',');
                        var result = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
                        bResult = result.Success;
                    }

                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "FeeRazorpayTransferAccount(string sRazorpayId, string sFeeRootId,string sAcademicYear)", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "FeeRazorpayTransferAccount(string sRazorpayId, string sFeeRootId,string sAcademicYear)", ex.Message);
                }

            }


            return bResult;
        }
        //test action "DO NOT USE THIS ACTION"
        //        public bool TestTransferAction() {
        //            Dictionary<string, object> options = new Dictionary<string, object>();
        //            //options.Add(Common.RazorpayKeyWords.TRANSFERS, lstTransfer);

        //            string ssql = @"SELECT 
        //    p.ID, p.UDF1
        //FROM
        //    fee_razorpay_payment_info_2019 AS p
        //        LEFT JOIN
        //    fee_razorpay_transfer_info_2019 AS t ON t.UDF2 = p.RAZORPAY_PAMENT_ID
        //WHERE
        //    t.RAZORPAY_TRANSFER_ID IS NULL;";
        //            var FetchRazorpayPaymentInfo = (List<FEE_RAZORPAY_PAYMENT_INFO>)CMSHandler.FetchData<FEE_RAZORPAY_PAYMENT_INFO>(null, ssql, "2019").DataSource.Data;

        //            if (FetchRazorpayPaymentInfo!=null && FetchRazorpayPaymentInfo.Count>0)
        //            {
        //                foreach (var paymentInfo in FetchRazorpayPaymentInfo)
        //                {
        //                    RazorpayClient client = new RazorpayClient("rzp_live_nyMdH0c02Rz2jq", "bJ4IdJ0kft3K9O6FA5SaPQTb");
        //                    Payment payments = client.Payment.Fetch(paymentInfo.ID);
        //                    List<Transfer> transfer = payments.Fetch(paymentInfo.ID).Transfers();
        //                    //Transfer tt = new Transfer().Fetch("");
        //                    //List<Transfer> transfer = payments.Transfer(options);
        //                    if (transfer != null && transfer.Count > 0)
        //                    {
        //                        string sSQL = @"INSERT INTO `FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR`
        //                                    (
        //                                    `ID`,
        //                                    `ENTITY`,
        //                                    `SOURCE`,
        //                                    `RECIPIENT`,
        //                                    `AMOUNT`,
        //                                    `CURRENCY`,
        //                                    `AMOUNT_REVERSED`,
        //                                    `FEES`,
        //                                    `TAX`,
        //                                    `ON_HOLD`,
        //                                    `ON_HOLD_UNTIL`,
        //                                    `RECIPIENT_SETTLEMENT_ID`,
        //                                    `CREATED_AT`,
        //                                    `UDF1`,
        //                                    `UDF2`,
        //                                    `UDF3`,
        //                                    `UDF4`,
        //                                    `UDF5`,
        //                                    `UDF6`)
        //                                    VALUES ";
        //                        foreach (var transferItem in transfer)
        //                        {
        //                            TransferItemResponse item = JsonConvert.DeserializeObject<TransferItemResponse>(transferItem.Attributes.ToString());
        //                            sSQL += @"('" + item.id + "','" + item.entity + "','" + item.source + "','" + item.recipient + "','" + item.amount + "','" + item.currency + "'" +
        //                                ",'" + item.amount_reversed + "','" + item.fees + "','" + item.tax + "','" + item.on_hold + "','" + item.on_hold_until + "'" +
        //                                ",'" + item.recipient_settlement_id + "','" + item.created_at + "','" + item.notes.udf1 + "','" + item.notes.udf2 + "','" + item.notes.udf3 + "'" +
        //                                ",'" + item.notes.udf4 + "','" + item.notes.udf5 + "','" + item.notes.udf6 + "'),";
        //                        }
        //                        sSQL = sSQL.TrimEnd(',');
        //                        var result = CMSHandler.SaveOrUpdate(null, sSQL, "2019");
        //                      //  bResult = result.Success;
        //                    }
        //                }
        //            }



        //            return true;
        //        }

        //This method to enter fee transactions 
        private FEE_RAZORPAY_PAYMENT_INFO FeeRazorpayCollection(FEE_RAZORPAY_PAYMENT_INFO objPaymentInfo, string sRazorpayPaymentId, string sAcademicYear)
        {
            try
            {
                var FetchFeeStudentAccount = new List<FEE_STUDENT_ACCOUNT>();
                string sStudentId = objPaymentInfo.UDF1;
                string sFrequencyId = objPaymentInfo.UDF3;
                string sFrequencyType = objPaymentInfo.UDF2;
                string sProgrammegroupid = objPaymentInfo.UDF6;
                string sAmount = (((!string.IsNullOrEmpty(objPaymentInfo.AMOUNT)) ? decimal.Parse(objPaymentInfo.AMOUNT) : 0) - ((!string.IsNullOrEmpty(objPaymentInfo.FEE)) ? decimal.Parse(objPaymentInfo.FEE) : 0)).ToString();
                string sSQL = string.Empty;
                if (sFrequencyType == Common.FrequencyType.HostelApplication || sFrequencyType == Common.FrequencyType.HostelFee)
                {
                    sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchHostelApplicationFeeStudentAccountByStudentIdAndFrequencyId).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFrequencyId);
                    FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = sStudentId }, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                {
                    sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFrequencyId);
                    FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = sStudentId, PROGRAMME = sProgrammegroupid }, sSQL, sAcademicYear).DataSource.Data;
                }
                if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                {
                    FetchFeeStudentAccount.RemoveAll(r => r.BALANCE == "0");
                    if (objPaymentInfo.CAPTURED.ToLower() == "true")
                    {
                        int ReceiptNo = 0;
                        string Prefix = "00";
                        string TransactionId = string.Empty;
                        FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                        FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                        AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                        if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                        {
                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            {
                                decimal d = FetchFeeStudentAccount.Sum(x => decimal.Parse(x.BALANCE) * 100);
                                if (decimal.Parse(sAmount) == FetchFeeStudentAccount.Sum(x => decimal.Parse(x.BALANCE) * 100))
                                {
                                    var GroupFeeStudentAccount = FetchFeeStudentAccount.GroupBy(x => x.FREQUENCY_ID);
                                    foreach (var FrequencyItem in GroupFeeStudentAccount)
                                    {
                                        var c = GroupFeeStudentAccount.Count();
                                        sFrequencyId = FrequencyItem.FirstOrDefault().FREQUENCY_ID;
                                        var feeTransactions = FetchFeeStudentAccount.Where(x => x.FREQUENCY_ID == sFrequencyId);
                                        decimal balance = 0;
                                        balance = feeTransactions.Sum(x => decimal.Parse(x.BALANCE));
                                        if (balance > 0)
                                        {
                                            objFeeTransactionModel.STUDENT_ID = sStudentId;
                                            objFeeTransactionModel.CLASS = string.Empty;
                                            objFeeTransactionModel.FREQUENCY = sFrequencyId;
                                            objFeeTransactionModel.FREQUENCY_TO = sFrequencyId;
                                            objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                            objFeeTransactionModel.USERNAME = objPaymentInfo.UDF1;
                                            objFeeTransactionModel.COLLECTED = feeTransactions.Sum(x => decimal.Parse(x.BALANCE)).ToString();
                                            var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                            if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                                ReceiptNo = Convert.ToInt32(FetchReceiptNo.FirstOrDefault().exam_receipt_no);
                                            else
                                            {
                                                // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                                //   objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                            }
                                            objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                            objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                            var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                            if (UpdateReceiptNo.Success)
                                            {
                                                objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Online;
                                                objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                                objPaymentInfo.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                                objFeeTransactionModel.RAZORPAY_ID = sRazorpayPaymentId;
                                                var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                                                if (SaveFeeTransaction.Success)
                                                {
                                                    //TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                                    var FetchTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(new FEE_TRANSACTION() { RAZORPAY_ID = sRazorpayPaymentId, FREQUENCY = sFrequencyId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByRazorpayId), sAcademicYear).DataSource.Data;
                                                    TransactionId = (FetchTransaction != null && FetchTransaction.Count > 0) ? FetchTransaction.FirstOrDefault().TRANSACTION_ID : SaveFeeTransaction.RowUniqueId.ToString();
                                                    //   feePayUResponse.txnid = TransactionId;
                                                    decimal dAmount = feeTransactions.Sum(x => decimal.Parse(x.BALANCE));
                                                    decimal dCredit = 0;
                                                    foreach (var item in feeTransactions)
                                                    {
                                                        if (dAmount > 0 && Convert.ToDecimal(item.BALANCE) > 0)
                                                        {
                                                            dCredit = Convert.ToDecimal(item.BALANCE);
                                                            item.DEBIT = (dAmount >= dCredit) ? item.BALANCE : (dCredit - dAmount).ToString();
                                                            dAmount = dAmount - dCredit;
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                        item.FREQUENCY = item.FREQUENCY_ID;
                                                        item.FEE_MAIN_HEAD_ID = item.FEE_MAIN_HEAD_ID;
                                                        item.TRANSACTION_ID = TransactionId;
                                                        item.PAID_AMOUNT = item.DEBIT;
                                                        item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                                        item.HEAD = item.HEAD_ID;
                                                        item.FEE_STRUCTURE_ID = item.FEE_STRUCTURE_ID;
                                                        item.FEE_ROOT_ID = item.FEE_ROOT_ID;


                                                        var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                                        if (SaveFeeCollection.Success)
                                                        {
                                                            var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                            if (SaveStudentAccount.Success)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                //objResult.Message = Common.Messages.FailedToSaveRecords;
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    objPaymentInfo.ERROR_CODE = "Pending";
                                                    objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                                                }
                                            }

                                        }
                                        else
                                        {

                                            objPaymentInfo.ERROR_CODE = "Pending";
                                            objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                                        }


                                    }
                                }
                                else
                                {
                                    objPaymentInfo.ERROR_CODE = "Pending";
                                    objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                                }
                            }
                            else
                            {
                                objPaymentInfo.ERROR_CODE = "Pending";
                                objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                            }
                        }
                        else
                        {
                            objPaymentInfo.ERROR_CODE = "Pending";
                            objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                        }

                    }
                    else
                    {
                        objPaymentInfo.ERROR_CODE = "Pending";
                        objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                    }

                }
                else
                {
                    objPaymentInfo.ERROR_CODE = "Pending";
                    objPaymentInfo.ERROR_DESCRIPTION = "Please note Transaction Id and enquire with admin !!";
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "FeeRazorpayCollection(FEE_RAZORPAY_PAYMENT_INFO objPaymentInfo, string sRazorpayPaymentId, string sAcademicYear)", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "FeeRazorpayCollection(FEE_RAZORPAY_PAYMENT_INFO objPaymentInfo, string sRazorpayPaymentId, string sAcademicYear)", ex.Message);
                }
            }
            return objPaymentInfo;
        }

        public bool FeeRazorpayRefund(string sPaymentId)
        {
            //FeeRazorpayTransferAccount           
            bool bResult = false;
            List<ADM_REFUND_STUDENT_ACCOUNT> studentAcountList = new List<ADM_REFUND_STUDENT_ACCOUNT>();
            ReversalResponse rvresponse = new ReversalResponse();
            int count = 0;
            try
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string key = string.Empty;
                string secret = string.Empty;
                var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), sAcademicYear).DataSource.Data;
                if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
                {
                    key = FetchMarchantAccount.FirstOrDefault().KEY;
                    secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                }
                studentAcountList = (List<ADM_REFUND_STUDENT_ACCOUNT>)CMSHandler.FetchData<ADM_REFUND_STUDENT_ACCOUNT>(new FEE_RAZORPAY_PAYMENT_INFO() { RAZORPAY_PAMENT_ID = sPaymentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicantRefundAmountForRefundIntiate), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (studentAcountList != null && studentAcountList.Count > 0)
                {

                    foreach (var item in studentAcountList)
                    {
                        string value = ReversalRequest(item.TRANSFER_ID, item.BALANCE);

                        rvresponse = JsonConvert.DeserializeObject<ReversalResponse>(((Newtonsoft.Json.Linq.JToken)value).Root.ToString());
                        var reversalResult = CMSHandler.SaveOrUpdate(rvresponse, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveStudentTransferReversalInfo), sAcademicYear);
                        if (reversalResult.Success)
                        {
                            count++;
                        }
                    }
                    if (studentAcountList.Count == count)
                    {
                        //refund Proccess
                    }
                    Notes note = new Notes();
                    // RefundMethod
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    string amount = studentAcountList.Sum(s => Convert.ToDouble(s.BALANCE)).ToString();
                    RazorpayClient rclient = new RazorpayClient(key, secret);
                    Payment payments = rclient.Payment.Fetch(studentAcountList.FirstOrDefault().ID);
                    note.udf1 = studentAcountList.FirstOrDefault().STUDENT_ID;
                    note.udf2 = studentAcountList.FirstOrDefault().ISSUED_ID;
                    note.udf3 = studentAcountList.FirstOrDefault().RAZORPAY_ID;
                    note.udf4 = studentAcountList.FirstOrDefault().UDF6;
                    data.Add("amount", (decimal.Parse(amount) * 100).ToString());
                    data.Add("currency", "INR");
                    data.Add("notes", note);
                    Refund refund = payments.Refund(data);
                    Refund objrefund = new Refund();
                    RefundResponse objrefundInfo = new RefundResponse();
                    //RefundResponse
                    objrefundInfo = JsonConvert.DeserializeObject<RefundResponse>(((Newtonsoft.Json.Linq.JToken)refund.Attributes).Root.ToString());
                    var o = (FEE_RAZORPAY_REFUND_INFO)CommonMethods.PropertyMapping<RefundResponse, FEE_RAZORPAY_REFUND_INFO>(objrefundInfo, null);
                    o.UDF1 = objrefundInfo.notes.udf1;
                    o.UDF2 = objrefundInfo.notes.udf2;
                    o.UDF3 = objrefundInfo.notes.udf3;
                    o.UDF4 = objrefundInfo.notes.udf4;
                    //RefundInsert
                    var restult = CMSHandler.SaveOrUpdate(o, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveStudentTransferRefundInfo), sAcademicYear);
                    if (restult.Success)
                    {
                        bResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "FeeRazorpayRefund", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "FeeRazorpayRefund", ex.Message);
                }

            }
            return bResult;
        }

        public string ReversalRequest(string sTransferId, string Amount)
        {
            string dataString = string.Empty;
            try
            {

                string key = string.Empty;
                string secret = string.Empty;
                var FetchMarchantAccount = (List<FEE_RAZORPAY_ACCOUNTS>)CMSHandler.FetchData<FEE_RAZORPAY_ACCOUNTS>(new FEE_RAZORPAY_ACCOUNTS() { RAZORPAY_ACCOUNT_TYPE_ID = Common.RazorpayAccountType.MarchantAccount }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRazorpayAccountInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (FetchMarchantAccount != null & FetchMarchantAccount.Count > 0)
                {
                    key = FetchMarchantAccount.FirstOrDefault().KEY;
                    secret = FetchMarchantAccount.FirstOrDefault().SECRET_KEY;
                }
                string Baseurl = "https://api.razorpay.com/v1/transfers/" + sTransferId + "/reversals";
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(key + ":" + secret));
                string paramenter = "{\r\n  \"amount\":" + (decimal.Parse(Amount) * 100).ToString() + "\r\n}";
                var data = Encoding.ASCII.GetBytes(paramenter);
                HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(Baseurl);
                requests.Headers.Add("Authorization", "Basic " + encoded);
                requests.Method = "POST";
                requests.Accept = "application/json; charset=utf-8";
                requests.ContentLength = data.Length;
                using (var stream = requests.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)requests.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                dataString = readStream.ReadToEnd();

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ReversalRequest(string sTransferId,string Amount)", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ReversalRequest(string sTransferId,string Amount)", ex.Message);
                }
            }
            return dataString;
        }


        #endregion


        #region FeeMainHead
        [UserRights("ADMIN")]
        public ActionResult ListFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(new FeeMainHead() { FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                    MainHeadView.SupShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift), sAcademicYear).DataSource.Data;
                    MainHeadView.SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    MainHeadView.SupFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeMode), sAcademicYear).DataSource.Data;
                    MainHeadView.LiSupFeeRoot = (List<SUP_FEE_ROOT>)CMSHandler.FetchData<SUP_FEE_ROOT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FectchFeeRoot), sAcademicYear).DataSource.Data;
                    MainHeadView.LiApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                    MainHeadView.SubHeads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList)).DataSource.Data;
                    MainHeadView.MainHeads = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHead)).DataSource.Data;
                    MainHeadView.LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts)).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListFeeMainHead", Ex.Message);
                }
            }
            return View(MainHeadView);
        }
        public ActionResult CollegeFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(new FeeMainHead() { FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                    MainHeadView.SupShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift), sAcademicYear).DataSource.Data;
                    MainHeadView.SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    MainHeadView.SupFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeMode), sAcademicYear).DataSource.Data;
                    MainHeadView.LiSupFeeRoot = (List<SUP_FEE_ROOT>)CMSHandler.FetchData<SUP_FEE_ROOT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FectchFeeRoot), sAcademicYear).DataSource.Data;
                    MainHeadView.LiApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                    MainHeadView.SubHeads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList)).DataSource.Data;
                    MainHeadView.MainHeads = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHead)).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", Ex.Message);
                }
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(MainHeadView);
        }
        public ActionResult BindCollegeFeeMainHead(string sFeeRoot, string sApplicationType, string sShift, string sProgrammeMode, string sFeeMode, string sMainHead, string sSubHeads)
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            string sSql = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                FeeMainHead objFeeMainHead = new FeeMainHead();
                try
                {
                    objFeeMainHead.FEE_ROOT_ID = sFeeRoot;
                    objFeeMainHead.SHIFT = sShift;
                    objFeeMainHead.PROGRAMME_MODE = sProgrammeMode;
                    objFeeMainHead.FREQUENCY_ID = sFeeMode;
                    objFeeMainHead.APPLICATION_TYPE_ID = sApplicationType;
                    objFeeMainHead.MAIN_HEAD_ID = sMainHead;
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeMainHeadBySubHead).Replace(Common.Delimiter.QUS + Common.SUP_HEAD.HEAD_ID, sSubHeads);
                    MainHeadView.LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts)).DataSource.Data;
                    MainHeadView.FeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, sSql, sAcademicYear).DataSource.Data;
                }
                catch (Exception Ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "BindCollegeFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "BindCollegeFeeMainHead", Ex.Message);
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");

            return View(MainHeadView);
        }
        //This method Removes the Mapped SubHead using this Conditions
        public JsonResult LoadSubheads(string sMainHead, string sShiftId, string sProgrammeModeId, string sFrequencyId, string sFeeRootId, string sApplicationTypeId)
        {
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    FeeMainHead objFeeMainHead = new FeeMainHead();
                    objFeeMainHead.MAIN_HEAD_ID = sMainHead;
                    objFeeMainHead.SHIFT = sShiftId;
                    objFeeMainHead.PROGRAMME_MODE = sProgrammeModeId;
                    objFeeMainHead.FREQUENCY_ID = sFrequencyId;
                    objFeeMainHead.FEE_ROOT_ID = sFeeRootId;
                    objFeeMainHead.APPLICATION_TYPE_ID = sApplicationTypeId;
                    var subheads = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSubHeadsByMainHeadId), sAcademicYear).DataSource.Data;
                    var MappedHeads = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMappedSupHead), sAcademicYear).DataSource.Data;
                    if (MappedHeads != null)
                    {
                        foreach (var Mapitem in MappedHeads)
                        {
                            subheads.RemoveAll(x => x.HEAD_ID == Mapitem.HEAD_ID);
                        }
                    }
                    if (subheads != null)
                    {
                        foreach (var item in subheads)
                        {
                            objResult.sResult += "<option value='" + item.HEAD_ID + "' " + item.STATUS + ">" + item.HEAD + "</option>";
                        }

                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "LoadSubheads", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "LoadSubheads", Ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateFeeMainHead(string sJsonFeeMainHeads)
        {
            JsonResultData objResult = new JsonResultData();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    FeeMainHead objModel = new FeeMainHead();
                    var FetchFeeMainHeadById = new List<FeeMainHead>();
                    var TempList = new List<FeeMainHead>();
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var obj = serializer.Deserialize<FEE_STRUCTURE_LIST>(sJsonFeeMainHeads);
                    foreach (var item in obj.JSON_FEE_MAIN_HEAD)
                    {
                        objModel.MAIN_HEAD_ID = item.MAIN_HEAD_ID;
                        objModel.SHIFT = item.SHIFT;
                        objModel.PROGRAMME_MODE = item.PROGRAMME_MODE;
                        objModel.FREQUENCY_ID = item.FREQUENCY_ID;
                        objModel.APPLICATION_TYPE_ID = item.APPLICATION_TYPE_ID;
                        objModel.FEE_ROOT_ID = item.FEE_ROOT_ID;
                    }
                    FetchFeeMainHeadById = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeadsByMainHeadId), sAcademicYear).DataSource.Data;
                    //this loop removes existing main heads 
                    if (FetchFeeMainHeadById != null && FetchFeeMainHeadById.Count() > 0)
                    {
                        if (obj.JSON_FEE_MAIN_HEAD != null && obj.JSON_FEE_MAIN_HEAD.Count() > 0)
                        {
                            foreach (var items in obj.JSON_FEE_MAIN_HEAD)
                            {
                                if (!string.IsNullOrEmpty(items.FEE_MAIN_HEAD_ID))
                                {
                                    items.ACADEMIC_YEAR = sAcademicYear;
                                    var SaveMainHead = CMSHandler.SaveOrUpdate(items, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeMainHead));
                                    if (SaveMainHead.Success)
                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                    else
                                        objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            foreach (var list in FetchFeeMainHeadById)
                            {
                                TempList.Add(obj.JSON_FEE_MAIN_HEAD.SingleOrDefault(S => S.MAIN_HEAD_ID == list.MAIN_HEAD_ID && S.HEAD_ID == list.HEAD_ID));
                                obj.JSON_FEE_MAIN_HEAD.Remove(obj.JSON_FEE_MAIN_HEAD.SingleOrDefault(S => S.MAIN_HEAD_ID == list.MAIN_HEAD_ID && S.HEAD_ID == list.HEAD_ID));
                            }
                            foreach (var item in TempList)
                            {
                                if (item != null)
                                    FetchFeeMainHeadById.Remove(FetchFeeMainHeadById.SingleOrDefault(S => S.MAIN_HEAD_ID == item.MAIN_HEAD_ID && S.HEAD_ID == item.HEAD_ID));
                            }
                            //this loop deletes records
                            foreach (var item in FetchFeeMainHeadById)
                            {
                                //Delete Records
                                item.ACADEMIC_YEAR = sAcademicYear;
                                var DeleteRecord = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsSubHeadMapped), sAcademicYear).DataSource.Data;
                                if (DeleteRecord.FirstOrDefault().STATUS != "1")
                                {
                                    var o = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeMainHeadByMainHeadIdAndHeadId));
                                    if (o.Success)
                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    objResult.Message = Common.Messages.LedgersAreMapped;
                                }

                            }
                        }
                    }
                    //this loop saves records
                    foreach (var item in obj.JSON_FEE_MAIN_HEAD)
                    {
                        item.ACADEMIC_YEAR = sAcademicYear;
                        var SaveMainHead = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeMainHead));
                        if (SaveMainHead.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "SaveFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "SaveFeeMainHead", Ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "LoadFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "LoadFeeMainHead", Ex.Message);
                    return View(MainHeadView);
                }
            }
            return View(MainHeadView);
        }
        public JsonResult DeleteFeeMainHead(string sFeeMainHead)
        {
            JsonResultData objResult = new JsonResultData();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    if (sFeeMainHead != null)
                    {
                        FeeMainHead objModel = new FeeMainHead();
                        objModel.FEE_MAIN_HEAD_ID = sFeeMainHead;
                        var MainHead = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeemainHeadbyId), sAcademicYear).DataSource.Data;
                        if (MainHead != null && MainHead.Count > 0)
                        {
                            objModel.FREQUENCY_ID = MainHead.FirstOrDefault().FREQUENCY_ID;
                            objModel.HEAD_ID = MainHead.FirstOrDefault().HEAD_ID;
                            var DeleteRecord = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsSubHeadMapped), sAcademicYear).DataSource.Data;
                            if (DeleteRecord != null && DeleteRecord.Count > 0)
                            {
                                if (DeleteRecord.FirstOrDefault().STATUS != "1")
                                {
                                    var o = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeMainHead));
                                    if (o.Success)
                                        objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                                }
                                else
                                {
                                    objResult.Message = Common.Messages.LedgersAreMapped;
                                }
                            }
                            else
                            {
                                objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                            }
                        }
                        else
                        {
                            objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                        }

                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "DeleteFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "DeleteFeeMainHead", Ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Hostel&MessFeeMainHead
        public ActionResult ListHostelFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(new FeeMainHead() { FEE_ROOT_ID = Common.FeeRoot.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListFeeMainHead", Ex.Message);
                }
            }
            return View(MainHeadView);
        }
        public ActionResult ListMessFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(new FeeMainHead() { FEE_ROOT_ID = Common.FeeRoot.MessFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListFeeMainHead", Ex.Message);
                }
            }
            return View(MainHeadView);
        }
        public ActionResult HostelFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    MainHeadView.SupFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.HostelFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    MainHeadView.LiSupFeeRoot = (List<SUP_FEE_ROOT>)CMSHandler.FetchData<SUP_FEE_ROOT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FectchFeeRoot), sAcademicYear).DataSource.Data;
                    MainHeadView.SubHeads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList)).DataSource.Data;
                    MainHeadView.MainHeads = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHead)).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", Ex.Message);
                }
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(MainHeadView);
        }
        public ActionResult BindHostelFeeMainHead(string sFeeRoot, string sFeeMode, string sMainHead, string sSubHeads)
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            string sSql = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                FeeMainHead objFeeMainHead = new FeeMainHead();
                try
                {
                    objFeeMainHead.FEE_ROOT_ID = sFeeRoot;
                    objFeeMainHead.FREQUENCY_ID = sFeeMode;
                    objFeeMainHead.MAIN_HEAD_ID = sMainHead;
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeMainHeadBySubHeadForHostel).Replace(Common.Delimiter.QUS + Common.SUP_HEAD.HEAD_ID, sSubHeads);
                    MainHeadView.LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts)).DataSource.Data;
                    MainHeadView.FeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                    MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, sSql, sAcademicYear).DataSource.Data;
                }
                catch (Exception Ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "BindCollegeFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "BindCollegeFeeMainHead", Ex.Message);
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");

            return View(MainHeadView);
        }
        public JsonResult LoadSubheadsForHostel(string sMainHead, string sFrequencyId, string sFeeRootId)
        {
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    FeeMainHead objFeeMainHead = new FeeMainHead();
                    objFeeMainHead.MAIN_HEAD_ID = sMainHead;
                    objFeeMainHead.FREQUENCY_ID = sFrequencyId;
                    objFeeMainHead.FEE_ROOT_ID = sFeeRootId;
                    var subheads = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSubHeadsByMainHeadIdForHostel), sAcademicYear).DataSource.Data;
                    var MappedHeads = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objFeeMainHead, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMappedSupHeadForHostel), sAcademicYear).DataSource.Data;
                    if (MappedHeads != null)
                    {
                        foreach (var Mapitem in MappedHeads)
                        {
                            subheads.RemoveAll(x => x.HEAD_ID == Mapitem.HEAD_ID);
                        }
                    }
                    if (subheads != null)
                    {
                        foreach (var item in subheads)
                        {
                            objResult.sResult += "<option value='" + item.HEAD_ID + "' " + item.STATUS + ">" + item.HEAD + "</option>";
                        }

                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "LoadSubheads", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "LoadSubheads", Ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateFeeMainHeadHostel(string sJsonFeeMainHeads)
        {
            JsonResultData objResult = new JsonResultData();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    FeeMainHead objModel = new FeeMainHead();
                    var FetchFeeMainHeadById = new List<FeeMainHead>();
                    var TempList = new List<FeeMainHead>();
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var obj = serializer.Deserialize<FEE_STRUCTURE_LIST>(sJsonFeeMainHeads);
                    foreach (var item in obj.JSON_FEE_MAIN_HEAD)
                    {
                        objModel.MAIN_HEAD_ID = item.MAIN_HEAD_ID;
                        objModel.FREQUENCY_ID = item.FREQUENCY_ID;
                        objModel.FEE_ROOT_ID = item.FEE_ROOT_ID;
                    }
                    FetchFeeMainHeadById = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeadsByMainHeadIdForHostel), sAcademicYear).DataSource.Data;
                    //this loop removes existing main heads 
                    if (FetchFeeMainHeadById != null && FetchFeeMainHeadById.Count() > 0)
                    {
                        if (obj.JSON_FEE_MAIN_HEAD != null && obj.JSON_FEE_MAIN_HEAD.Count() > 0)
                        {
                            foreach (var items in obj.JSON_FEE_MAIN_HEAD)
                            {
                                if (!string.IsNullOrEmpty(items.FEE_MAIN_HEAD_ID))
                                {
                                    items.ACADEMIC_YEAR = sAcademicYear;
                                    var SaveMainHead = CMSHandler.SaveOrUpdate(items, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeMainHead));
                                    if (SaveMainHead.Success)
                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                    else
                                        objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            foreach (var list in FetchFeeMainHeadById)
                            {
                                TempList.Add(obj.JSON_FEE_MAIN_HEAD.SingleOrDefault(S => S.MAIN_HEAD_ID == list.MAIN_HEAD_ID && S.HEAD_ID == list.HEAD_ID));
                                obj.JSON_FEE_MAIN_HEAD.Remove(obj.JSON_FEE_MAIN_HEAD.SingleOrDefault(S => S.MAIN_HEAD_ID == list.MAIN_HEAD_ID && S.HEAD_ID == list.HEAD_ID));
                            }
                            foreach (var item in TempList)
                            {
                                if (item != null)
                                    FetchFeeMainHeadById.Remove(FetchFeeMainHeadById.SingleOrDefault(S => S.MAIN_HEAD_ID == item.MAIN_HEAD_ID && S.HEAD_ID == item.HEAD_ID));
                            }
                            //this loop deletes records
                            foreach (var item in FetchFeeMainHeadById)
                            {
                                //Delete Records
                                item.ACADEMIC_YEAR = sAcademicYear;
                                var DeleteRecord = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsSubHeadMapped), sAcademicYear).DataSource.Data;
                                if (DeleteRecord.FirstOrDefault().STATUS != "1")
                                {
                                    var o = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeMainHeadByMainHeadIdAndHeadId));
                                    if (o.Success)
                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    objResult.Message = Common.Messages.LedgersAreMapped;
                                }

                            }
                        }
                    }
                    //this loop saves records
                    foreach (var item in obj.JSON_FEE_MAIN_HEAD)
                    {
                        item.ACADEMIC_YEAR = sAcademicYear;
                        var SaveMainHead = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeMainHeadForHostel));
                        if (SaveMainHead.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "SaveFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "SaveFeeMainHead", Ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MessFeeMainHead()
        {
            FeeMainHeadViewModel MainHeadView = new FeeMainHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    // MainHeadView.FeeMainHeadList = (List<FeeMainHead>)CMSHandler.FetchData<FeeMainHead>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeads), sAcademicYear).DataSource.Data;
                    // MainHeadView.SupShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift), sAcademicYear).DataSource.Data;
                    //MainHeadView.SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    MainHeadView.SupFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.MessFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    MainHeadView.LiSupFeeRoot = (List<SUP_FEE_ROOT>)CMSHandler.FetchData<SUP_FEE_ROOT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FectchFeeRoot), sAcademicYear).DataSource.Data;
                    //MainHeadView.LiSupApplictionType = (List<SUP_APPLICTION_TYPE>)CMSHandler.FetchData<SUP_APPLICTION_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                    MainHeadView.SubHeads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList)).DataSource.Data;
                    MainHeadView.MainHeads = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHead)).DataSource.Data;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "CollegeFeeMainHead", Ex.Message);
                }
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(MainHeadView);
        }
        #endregion

        #region Student Transaction And Account
        //[UserRights("EXAMINCHARGE")]
        public ActionResult ListStudentAccount()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllFrequency), sAcademicYear).DataSource.Data;
            // var FetchStudent = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllStudent)).DataSource.Data;
            var classes = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            if (FetchFrequency != null && FetchFrequency.Count > 0)
                objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
            //if (FetchStudent != null && FetchStudent.Count > 0)
            //  objViewModel.liStudentPersonalInfo = new SelectList(FetchStudent, Common.STU_PERSONAL_INFO.STUDENT_ID, Common.STU_PERSONAL_INFO.FIRST_NAME);
            if (classes != null && classes.Count > 0)
                objViewModel.liClassList = new SelectList(classes, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            return View(objViewModel);
        }

        public JsonResult BindStudentListByClassId(string sClassId)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sOption = string.Empty;
                var studentList = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(new cp_Classes_2017() { CLASS_ID = sClassId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByClassID), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (studentList != null && studentList.Count > 0)
                {
                    sOption = "<option value='0'>--Select--</option>";
                    foreach (var item in studentList)
                    {
                        sOption += "<option value='" + item.STUDENT_ID + "'>" + item.FIRST_NAME + "</option>";
                    }
                    objResult.sResult = sOption;
                }
            }
            else
            {
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindStudnetAccountDetails(string sFrequencyId, string sStudentId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
            objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
            objFeeStudentAccountModel.STUDENT_ID = sStudentId;
            var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objFeeStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
            var FetchCreditAndBalance = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchCreditByStudentId), sAcademicYear).DataSource.Data;
            var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                objViewModel.liFeeStudentAccount = FetchFeeStudentAccount;
            if (FetchCreditAndBalance != null && FetchCreditAndBalance.Count > 0)
                objViewModel.liCreditAndDebit = FetchCreditAndBalance;
            if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                objViewModel.lstStudentPersonalInfo = FetchStudentInfo;
            return View(objViewModel);
        }
        public JsonResult SaveStudentAccount(string sStuTransaction, string sFrequencyId, string sStudentId, string sTotal)
        {
            JsonResultData objResult = new JsonResultData();
            try
            {
                int ReceiptNo = 0;
                string Prefix = "00";
                string TransactionId = string.Empty;
                FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonFeeStudentAccountList objJson = JsonConvert.DeserializeObject<JsonFeeStudentAccountList>(sStuTransaction);
                    objStudentAccountModel.STUDENT_ID = sStudentId;
                    var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                    if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                    {
                        objFeeTransactionModel.STUDENT_ID = FetchStudentInfo.FirstOrDefault().STUDENT_ID;
                        objFeeTransactionModel.CLASS = FetchStudentInfo.FirstOrDefault().CLASS_ID;
                        objFeeTransactionModel.FREQUENCY = sFrequencyId;
                        objFeeTransactionModel.FREQUENCY_TO = sFrequencyId;
                        objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                        objFeeTransactionModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_CODE].ToString();
                        objFeeTransactionModel.COLLECTED = sTotal;
                        var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                        if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                            foreach (var item in FetchReceiptNo)
                            {
                                ReceiptNo = Convert.ToInt32(item.exam_receipt_no);
                            }
                        else
                        {
                            objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                            objResult.Message = Common.ErrorMessage.ExpectationFailed;
                        }
                        objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                        objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                        var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                        if (UpdateReceiptNo.Success)
                        {
                            objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Cash;
                            objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                            var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                            if (SaveFeeTransaction.Success)
                            {
                                TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                objResult.sResult = TransactionId;
                                foreach (var item in objJson.StudentAccountList)
                                {
                                    item.FREQUENCY = item.FREQUENCY_ID;
                                    item.TRANSACTION_ID = TransactionId;
                                    item.PAID_AMOUNT = item.DEBIT;
                                    item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                    var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                    if (SaveFeeCollection.Success)
                                    {
                                        var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                        if (SaveStudentAccount.Success)
                                        {
                                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                        }
                                        else
                                        {
                                            objResult.Message = Common.Messages.FailedToSaveRecords;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                objResult.Message = Common.ErrorMessage.ExpectationFailed;
                            }
                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                        objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                    }

                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListStudentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListStudentAccount", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateFeeReceipt(string sFrequencyId, string sStudentId, string sTransactionId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                    objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                    objFeeStudentAccountModel.STUDENT_ID = sStudentId;
                    objFeeStudentAccountModel.TRANSACTION_ID = sTransactionId;
                    var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objFeeStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                    var FetchTotalBalance = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTotalDebitByTransactionId)).DataSource.Data;
                    if (FetchTotalBalance != null && FetchTotalBalance.Count > 0)
                        Number = Convert.ToInt32(FetchTotalBalance.FirstOrDefault().COLLECTED);
                    var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByTransactionId), sAcademicYear).DataSource.Data;
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                    if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                        objViewModel.lstStudentPersonalInfo = FetchStudentInfo;
                    if (FetchTotalBalance != null && FetchTotalBalance.Count > 0)
                        objViewModel.liFeeTransaction = FetchTotalBalance;
                    if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                        objViewModel.liFeeStudentAccount = FetchFeeStudentAccount;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "GenerateFeeReceipt", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "GenerateFeeReceipt", Ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion
        #region Class Wise Paid Status
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListClassWiseStudentPaidStatus()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllFrequency), sAcademicYear).DataSource.Data;
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.liClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListStudentAccount", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListStudentAccount", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindClassWiseFeeDetails(string sFrequencyId, string sClassId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            objFeeStudentAccountModel.FREQUENCY = sFrequencyId;
            objFeeStudentAccountModel.CLASS = sClassId;
            var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
            var FetchClassWiseFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchClassWiseFeeDetails), sAcademicYear).DataSource.Data;
            if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                objViewModel.liFeeTransaction = FetchClassWiseFeeDetails;
            if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                objViewModel.liCollegeDetails = FetchCollegeDetails;
            return View(objViewModel);
        }
        #endregion
        #region Monthly Status For Student Exam Fee Payment
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListMonthlyStatusForExamFeePayment()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllFrequency), sAcademicYear).DataSource.Data;
                    var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                        objViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListStudentAccount", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListStudentAccount", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindMonthWiseFeeDetails(string sFrequencyId, string sProgrammeMode, string sDateFrom, string sDateTo)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            objFeeStudentAccountModel.FREQUENCY = sFrequencyId;
            objFeeStudentAccountModel.PROGRAMME_MODE = sProgrammeMode;
            objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
            objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
            var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
            var FetchClassWiseFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMonthlyStudentFeeDetails), sAcademicYear).DataSource.Data;
            var FetchMonth = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentFeeDetailsOnlyMonth), sAcademicYear).DataSource.Data;
            if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                objViewModel.liFeeTransaction = FetchClassWiseFeeDetails;
            if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                objViewModel.liCollegeDetails = FetchCollegeDetails;
            if (FetchMonth != null && FetchMonth.Count > 0)
                objViewModel.liMonth = FetchMonth;
            return View(objViewModel);
        }

        public ActionResult FeeSummary(string sFrequencyId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sFrequencyId))
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    string sStudentId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                    objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                    objFeeStudentAccountModel.STUDENT_ID = sStudentId;
                    var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objFeeStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                    var FetchTotalBalance = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeSummaryByStudentId), sAcademicYear).DataSource.Data;
                    Number = Convert.ToInt32(FetchTotalBalance.FirstOrDefault().DEBIT);
                    // var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByTransactionId), sAcademicYear).DataSource.Data;
                    objViewModel.lstStudentPersonalInfo = FetchStudentInfo;
                    objViewModel.liFeeStudentAccount = FetchTotalBalance;
                }
                else
                {
                    objViewModel.sMessage = "Invalid input(s) Please enquire with admin !!!";
                }
            }
            else
            {
                objViewModel.sMessage = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }

            return View(objViewModel);
        }
        #endregion
        #region Bulk Fee Collection
        public ActionResult ListBulkFeeCollection()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllFrequency), sAcademicYear).DataSource.Data;
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.liClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListBulkFeeCollection", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListBulkFeeCollection", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindBulkFeeCollectionDetails(string sFrequencyId, string sClassId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            objFeeStudentAccountModel.FREQUENCY = sFrequencyId;
            objFeeStudentAccountModel.CLASS = sClassId;
            var FetchBulkFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchClassWiseFeeDetails), sAcademicYear).DataSource.Data;
            if (FetchBulkFeeDetails != null && FetchBulkFeeDetails.Count > 0)
                objViewModel.liFeeTransaction = FetchBulkFeeDetails;
            return View(objViewModel);
        }
        public JsonResult SaveBulkFeeCollection(string sStudentId)
        {
            JsonResultData objResult = new JsonResultData();
            try
            {
                int ReceiptNo = 0;
                string Prefix = "00";
                string TransactionId = string.Empty;
                FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonFeeTransactionList objJson = JsonConvert.DeserializeObject<JsonFeeTransactionList>(sStudentId);
                    if (sStudentId != null)
                    {
                        foreach (var item in objJson.FeeTransactionList)
                        {
                            objStudentAccountModel.STUDENT_ID = item.STUDENT_ID;
                            var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                            if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                            {
                                objFeeTransactionModel.STUDENT_ID = FetchStudentInfo.FirstOrDefault().STUDENT_ID;
                                objFeeTransactionModel.CLASS = FetchStudentInfo.FirstOrDefault().CLASS_ID;
                                objFeeTransactionModel.FREQUENCY = item.FREQUENCY;
                                objFeeTransactionModel.FREQUENCY_TO = item.FREQUENCY;
                                objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                objFeeTransactionModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_CODE].ToString();
                                objFeeTransactionModel.COLLECTED = item.COLLECTED;
                                var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                    ReceiptNo = Convert.ToInt32(FetchReceiptNo.SingleOrDefault().exam_receipt_no);
                                else
                                {
                                    objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                }
                                objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                if (UpdateReceiptNo.Success)
                                {
                                    objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Cash;
                                    objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                    var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                                    if (SaveFeeTransaction.Success)
                                    {
                                        TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                        objResult.sResult += "," + TransactionId;
                                        objStudentAccountModel.FREQUENCY_ID = item.FREQUENCY;
                                        var FetchStudentAccountOnlyCredit = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountOnlyCredit), sAcademicYear).DataSource.Data;
                                        if (FetchStudentAccountOnlyCredit != null && FetchStudentAccountOnlyCredit.Count > 0)
                                        {
                                            foreach (var dataitem in FetchStudentAccountOnlyCredit)
                                            {
                                                objStudentAccountModel.PAID_AMOUNT = dataitem.CREDIT;
                                                objStudentAccountModel.HEAD = dataitem.HEAD;
                                                objStudentAccountModel.TRANSACTION_ID = TransactionId;
                                                objStudentAccountModel.FREQUENCY = item.FREQUENCY;
                                                objStudentAccountModel.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                                objStudentAccountModel.DEBIT = dataitem.CREDIT;
                                                var SaveFeeCollection = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                                if (SaveFeeCollection.Success)
                                                {
                                                    var SaveStudentAccount = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                    if (SaveStudentAccount.Success)
                                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                                    else
                                                    {
                                                        var DeleteFeeStudentAccount = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccount));
                                                        if (DeleteFeeStudentAccount.Success)
                                                        {
                                                            var DeleteFeeCollection = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeCollection));
                                                            if (DeleteFeeCollection.Success)
                                                            {
                                                                var DeleteFeeTransaction = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeTransaction));
                                                                if (DeleteFeeTransaction.Success)
                                                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var DeleteFeeStudentAccount = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccount));
                                                    if (DeleteFeeStudentAccount.Success)
                                                    {
                                                        var DeleteFeeCollection = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeCollection));
                                                        if (DeleteFeeCollection.Success)
                                                        {
                                                            var DeleteFeeTransaction = CMSHandler.SaveOrUpdate(objStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeTransaction));
                                                            if (DeleteFeeTransaction.Success)
                                                                objResult.Message = Common.Messages.FailedToSaveRecords;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                            objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                        }
                                    }
                                    else
                                        objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                                else
                                {
                                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                                }
                            }
                            else
                            {
                                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                            }
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
                objResult.sResult = objResult.sResult.Substring(1);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListBulkFeeCollection", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListBulkFeeCollection", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateBulkFeeReceipt(string sStudentId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            var StudentInfo = new List<Student_Personal_Info>();
            var FetchCollegeDetails = new List<CollegeDetails>();
            var BalanceInfo = new List<FEE_TRANSACTION>();
            var FeeStudentAccount = new List<FEE_STUDENT_ACCOUNT>();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonFeeTransactionList objJson = JsonConvert.DeserializeObject<JsonFeeTransactionList>(sStudentId);
                    foreach (var item in objJson.FeeTransactionList)
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                        var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                        FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                        var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(item, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                        foreach (var list in FetchStudentInfo)
                            StudentInfo.Add(list);
                        var FetchTotalBalance = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTotalDebitByTransactionId)).DataSource.Data;
                        foreach (var dataitem in FetchTotalBalance)
                        {
                            if (FetchTotalBalance != null && FetchTotalBalance.Count > 0)
                                Number = Convert.ToInt32(FetchTotalBalance.FirstOrDefault().COLLECTED);
                            BalanceInfo.Add(dataitem);
                        }
                        var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(item, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByTransactionId), sAcademicYear).DataSource.Data;
                        foreach (var listitem in FetchFeeStudentAccount)
                            FeeStudentAccount.Add(listitem);
                    }
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                    if (StudentInfo != null && StudentInfo.Count > 0)
                        objViewModel.lstStudentPersonalInfo = StudentInfo;
                    if (BalanceInfo != null && BalanceInfo.Count > 0)
                        objViewModel.liFeeTransaction = BalanceInfo;
                    if (FeeStudentAccount != null && FeeStudentAccount.Count > 0)
                        objViewModel.liFeeStudentAccount = FeeStudentAccount;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "GenerateBulkFeeReceipt", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "GenerateBulkFeeReceipt", Ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult GenerateIndividualFeeReceipt(string sFrequencyId, string sStudentId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                    objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                    objFeeStudentAccountModel.FREQUENCY = sFrequencyId;
                    objFeeStudentAccountModel.STUDENT_ID = sStudentId;
                    var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objFeeStudentAccountModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                    var FetchTotalBalance = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTotalDebitByStudentIdAndFrequencyId)).DataSource.Data;
                    if (FetchTotalBalance != null && FetchTotalBalance.Count > 0)
                        Number = Convert.ToInt32(FetchTotalBalance.FirstOrDefault().COLLECTED);
                    var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountForReceiptByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                    if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                        objViewModel.lstStudentPersonalInfo = FetchStudentInfo;
                    if (FetchTotalBalance != null && FetchTotalBalance.Count > 0)
                        objViewModel.liFeeTransaction = FetchTotalBalance;
                    if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                        objViewModel.liFeeStudentAccount = FetchFeeStudentAccount;
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "GenerateIndividualFeeReceipt", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "GenerateIndividualFeeReceipt", Ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region AdmissionFeeCollectionInOffline
        [UserRights("GUEST")]
        public ActionResult OfflineCollectionForAdmission()
        {
            OfflineCollectionViewModel offlineCollectionModel = new OfflineCollectionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                offlineCollectionModel.VerifiedList = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.VerifiedStudentList), sAcademicYear).DataSource.Data;
                offlineCollectionModel.FeeTypeList = (List<sup_frequency_type>)CMSHandler.FetchData<sup_frequency_type>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeFrequencyByAcademicyear), sAcademicYear).DataSource.Data;
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(offlineCollectionModel);
        }

        #endregion

        #region PayUIntegration
        [UserRights("ADMIN")]
        public ActionResult PayURefundList()
        {

            var result = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchRefundList)).DataSource.Data;
            return View(result);
        }
        public ActionResult PayURepsponse(FormCollection payUResponse)
        {
            fee_payu_response feePayUResponse = new fee_payu_response();
            try
            {
                if (payUResponse != null)
                {
                    JsonResultData objResult = new JsonResultData();
                    string PayUResponse_Id = string.Empty;
                    string status = payUResponse["status"].ToString(); feePayUResponse = FormCollectionToClass(payUResponse);
                    objResult.oResult = feePayUResponse;
                    var isExisting = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayUResponseExist)).DataSource.Data;
                    if (isExisting != null && isExisting.Count > 0)
                    {
                        feePayUResponse.PAYU_RESPONSE_ID = isExisting.FirstOrDefault().PAYU_RESPONSE_ID;
                        var updateState = CMSHandler.SaveOrUpdate(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdatePayUResponse));
                        return View(feePayUResponse);
                    }
                    var result = CMSHandler.SaveOrUpdate(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayUResponse), "", true);
                    PayUResponse_Id = (result.RowUniqueId != null) ? result.RowUniqueId.ToString() : string.Empty;
                    if (status.Equals("success"))
                    {
                        string[] merc_hash_vars_seq;
                        string merc_hash_string = string.Empty;
                        string merc_hash = string.Empty;
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        string sStudentId = payUResponse["udf1"].ToString();
                        string sFrequencyId = payUResponse["udf2"].ToString();
                        var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = sFrequencyId, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                        var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { API_TYPE = Common.PayUMoneyAPIsType.BaseUrl, ACCOUNT_TYPE = Common.FrequencyType.ExamFee, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUPaymentAccountInfo)).DataSource.Data;
                        if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                        {
                            if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                            {
                                merc_hash_vars_seq = payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE.Split('|');
                                Array.Reverse(merc_hash_vars_seq);
                                if (payUResponse.AllKeys.Contains("additionalCharges"))
                                {
                                    merc_hash_string = payUResponse["additionalCharges"].ToString() + "|";
                                }
                                merc_hash_string += payUPaymentInfo.FirstOrDefault().SALT + "|" + payUResponse["status"].ToString();
                                foreach (var merc_hash_var in merc_hash_vars_seq)
                                {
                                    merc_hash_string += "|";
                                    merc_hash_string = merc_hash_string + (payUResponse[merc_hash_var] != null ? payUResponse[merc_hash_var] : "");
                                }
                                merc_hash = CommonMethods.Generatehash512(merc_hash_string).ToLower();
                                if (merc_hash == payUResponse["hash"])
                                {
                                    int ReceiptNo = 0;
                                    string Prefix = "00";
                                    string TransactionId = string.Empty;
                                    FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                                    FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                                    AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                                    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                                    {
                                        var FetchStudentInfo = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(FetchFeeStudentAccount.FirstOrDefault(), SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByStudentId), sAcademicYear).DataSource.Data;
                                        if (FetchStudentInfo != null && FetchStudentInfo.Count > 0)
                                        {
                                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                                            {
                                                decimal balance = 0;
                                                decimal.TryParse(FetchFeeStudentAccount.FirstOrDefault().BALANCE, out balance);
                                                if (balance > 0)
                                                {
                                                    objFeeTransactionModel.STUDENT_ID = FetchStudentInfo.FirstOrDefault().STUDENT_ID;
                                                    objFeeTransactionModel.CLASS = FetchStudentInfo.FirstOrDefault().CLASS_ID;
                                                    objFeeTransactionModel.FREQUENCY = sFrequencyId;
                                                    objFeeTransactionModel.FREQUENCY_TO = sFrequencyId;
                                                    objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                                    objFeeTransactionModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_CODE].ToString();
                                                    objFeeTransactionModel.COLLECTED = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE)).ToString();
                                                    var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                                    if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                                        ReceiptNo = Convert.ToInt32(FetchReceiptNo.FirstOrDefault().exam_receipt_no);
                                                    else
                                                    {
                                                        objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                                        objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                                    }
                                                    objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                                    objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                                    var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                                    if (UpdateReceiptNo.Success)
                                                    {
                                                        objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Online;
                                                        objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                                        objFeeTransactionModel.PayUResponse_Id = PayUResponse_Id;
                                                        var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                                                        if (SaveFeeTransaction.Success)
                                                        {
                                                            TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                                            objResult.sResult = TransactionId;
                                                            foreach (var item in FetchFeeStudentAccount)
                                                            {
                                                                item.DEBIT = item.CREDIT;
                                                                item.FREQUENCY = item.FREQUENCY_ID;
                                                                item.TRANSACTION_ID = TransactionId;
                                                                item.PAID_AMOUNT = item.DEBIT;
                                                                item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                                                item.HEAD = item.HEAD_ID;

                                                                var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                                                if (SaveFeeCollection.Success)
                                                                {
                                                                    var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                                    if (SaveStudentAccount.Success)
                                                                    {
                                                                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                                                    }
                                                                    else
                                                                    {
                                                                        objResult.Message = Common.Messages.FailedToSaveRecords;
                                                                    }
                                                                }
                                                            }
                                                            payUResponse.Clear();
                                                        }
                                                        else
                                                        {
                                                            objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                                            objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    feePayUResponse.status = "Pending";
                                                    feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                                                }
                                            }
                                            else
                                            {
                                                feePayUResponse.status = "Pending";
                                                feePayUResponse.error_Message = "Registration failed";
                                            }

                                        }
                                        else
                                        {
                                            objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                                            objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                                        }

                                    }
                                    else
                                    {
                                        objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                                        objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                                    }

                                }
                                else
                                {
                                    feePayUResponse.status = "Pending";
                                    feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                                }
                            }
                            else
                            {
                                feePayUResponse.status = "Pending";
                                feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                            }
                        }
                        else
                        {
                            feePayUResponse.status = "Pending";
                            feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                        }
                    }
                    else
                    {
                        feePayUResponse.status = "Failed";
                        feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                    }
                }
                else
                {
                    feePayUResponse.status = "Failed";
                    feePayUResponse.error_Message = "Null response, Please enquire with admin !!";
                }


            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    var LineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                    objHandler.WriteError("Fee", "PayURepsponse", "Err MSg: " + ex.Message, "LineNumber:" + LineNumber);
                }
                feePayUResponse.error_Message = "Sorry for the trouble,Please note Transaction Id and enquire with admin !!";
                return View(feePayUResponse);

            }
            return View(feePayUResponse);
        }

        public JsonResult PayURequest(string objInput)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_PAYU_REQUEST obj = JsonConvert.DeserializeObject<FEE_PAYU_REQUEST>(objInput);
            var fresult = (List<FEE_PAYU_REQUEST>)CMSHandler.FetchData<FEE_PAYU_REQUEST>(obj, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayURequestExist)).DataSource.Data;
            if (fresult != null && fresult.Count > 0)
            {
                objResult.Message = "Sorry !! Invalid attempt, try again.Please enquire with admin For more information.";
                return Json(objResult);
            }
            var result = CMSHandler.SaveOrUpdate(obj, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayURequest));
            objResult.Message = "";
            if (!result.Success)
                objResult.Message = "Sorry !! Failed to process, Please enquire with admin.";
            return Json(objResult);
        }

        public ActionResult PaymentOption(string sStudentId, string sFrequencyId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                PayUPost objPayUPost = new PayUPost();
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    decimal TotalAmount = 0;
                    var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = sFrequencyId, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                    var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { API_TYPE = Common.PayUMoneyAPIsType.BaseUrl, ACCOUNT_TYPE = Common.FrequencyType.ExamFee, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUPaymentAccountInfo)).DataSource.Data;
                    if (!string.IsNullOrEmpty(sStudentId) && !string.IsNullOrEmpty(sFrequencyId))
                    {
                        if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                        {
                            TotalAmount = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE));

                            if (TotalAmount > 0)
                            {
                                if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                                {
                                    objPayUPost.productinfo = "Exam Fee";
                                    objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
                                    objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
                                    objPayUPost.udf1 = payUPaymentInfo.FirstOrDefault().STUDENT_ID;
                                    objPayUPost.email = payUPaymentInfo.FirstOrDefault().STU_EMAILID;
                                    objPayUPost.udf5 = payUPaymentInfo.FirstOrDefault().STU_MOBILENO;
                                    objPayUPost.firstname = payUPaymentInfo.FirstOrDefault().FIRST_NAME;
                                    objPayUPost.udf3 = payUPaymentInfo.FirstOrDefault().ROLL_NO.Trim();
                                    objPayUPost.udf4 = payUPaymentInfo.FirstOrDefault().REGISTER_NO.Trim();
                                    objPayUPost.udf2 = sFrequencyId;
                                    objPayUPost.txnid = CommonMethods.CommonTransactionId(sStudentId);
                                    objPayUPost.amount = Convert.ToDecimal(TotalAmount).ToString("g29");
                                    objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);
                                    objPayUPost.surl = payUPaymentInfo.FirstOrDefault().surl;
                                    objPayUPost.furl = payUPaymentInfo.FirstOrDefault().furl;
                                    objPayUPost.curl = payUPaymentInfo.FirstOrDefault().curl;
                                    objPayUPost.phone = payUPaymentInfo.FirstOrDefault().STU_MOBILENO;
                                    objPayUPost.BASE_URL = payUPaymentInfo.FirstOrDefault().BASE_URL.Trim();
                                    return View(objPayUPost);
                                }
                                else
                                {
                                    objPayUPost.sMessage = "Missing Fields,Please enquire with Admin..!!!";
                                }
                            }
                            else
                            {
                                objPayUPost.sMessage = "Sorry !!! Invalid Amount.. Please enquire with Admin!!!";
                            }

                        }
                        else
                        {
                            objPayUPost.sMessage = "Not Registered ,Please enquire with Admin..!!!";

                        }
                    }
                    else
                    {
                        objPayUPost.sMessage = "Missing Fields, Please enquire with Admin..!!!";
                    }

                    return View(objPayUPost);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "PaymentOption(string sStudentId, string sFrequencyId)", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "PaymentOption(string sStudentId, string sFrequencyId)", ex.Message);
                    }
                    objPayUPost.sMessage = " Exception Occured...Sorry for the trouble,Please enquire with Admin..!!!";
                    return View(objPayUPost);
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

        }
        private fee_payu_response FormCollectionToClass(FormCollection collection)
        {

            fee_payu_response obj = new fee_payu_response();
            foreach (var key in collection.AllKeys)
            {
                Type t = obj.GetType();
                System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, collection[key], null);
                }

            }
            return obj;
        }
        public JsonResult GetHashValue(string sStudentId, string sFrequencyId, string sEmail, string sMobile, string sTxnid)
        {
            JsonResultData objResultData = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                PayUPost objPayUPost = new PayUPost();
                if (!string.IsNullOrEmpty(sStudentId) && !string.IsNullOrEmpty(sFrequencyId) && !string.IsNullOrEmpty(sEmail) && !string.IsNullOrEmpty(sMobile))
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = sFrequencyId, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                    var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { API_TYPE = Common.PayUMoneyAPIsType.BaseUrl, ACCOUNT_TYPE = Common.FrequencyType.ExamFee, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUPaymentAccountInfo)).DataSource.Data;

                    if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                    {
                        if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                        {
                            objPayUPost.productinfo = "Exam Fee";
                            objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
                            objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
                            objPayUPost.udf1 = payUPaymentInfo.FirstOrDefault().STUDENT_ID;
                            objPayUPost.email = sEmail.Trim();
                            objPayUPost.udf5 = payUPaymentInfo.FirstOrDefault().STU_MOBILENO;
                            objPayUPost.firstname = payUPaymentInfo.FirstOrDefault().FIRST_NAME;
                            objPayUPost.udf3 = payUPaymentInfo.FirstOrDefault().ROLL_NO.Trim();
                            objPayUPost.udf4 = payUPaymentInfo.FirstOrDefault().REGISTER_NO.Trim();
                            objPayUPost.udf2 = sFrequencyId;
                            objPayUPost.txnid = sTxnid;
                            objPayUPost.phone = sMobile.Trim();
                            objPayUPost.amount = Convert.ToDecimal(FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE))).ToString("g29");
                            objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);
                            objResultData.sResult = objPayUPost.hash;
                            objResultData.ErrorCode = Common.ErrorCode.Ok;
                        }
                        else
                        {
                            objResultData.ErrorCode = Common.ErrorCode.FailedDependency;
                            objResultData.Message = "Missing Fields,Please Admin..!!!";
                        }
                    }
                    else
                    {
                        objResultData.ErrorCode = Common.ErrorCode.FailedDependency;
                        objResultData.Message = "Not Registered ,Please Admin..!!!";
                    }
                }
                else
                {
                    objResultData.ErrorCode = Common.ErrorCode.FailedDependency;
                    objResultData.Message = Common.ErrorMessage.FailedDependency;
                }
            }
            else
            {
                objResultData.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResultData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResultData);
        }

        private string GetHashValue(string hashSeq, string sMerchant_key, PayUPost objPost)
        {
            string hash_string = string.Empty;
            string hash = string.Empty;
            string[] hashVarsSeq = hashSeq.Split('|'); // spliting hash sequence from config       
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + sMerchant_key;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + objPost.txnid;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + Convert.ToDecimal(objPost.amount).ToString("g29");
                    hash_string = hash_string + '|';
                }
                else
                {
                    Type t = objPost.GetType();
                    hash_string = hash_string + (objPost.GetType().GetProperty(hash_var).GetValue(objPost, null) != null ? objPost.GetType().GetProperty(hash_var).GetValue(objPost, null) : "");// isset if else
                    hash_string = hash_string + '|';
                }
            }

            hash_string += objPost.salt;// appending SALT

            hash = CommonMethods.Generatehash512(hash_string).ToLower();         //generating hash
            return hash;
        }
        #endregion
        #region Fee Structure
        [UserRights("ADMIN")]
        public ActionResult FeeStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var FetchProgrammeGroup = (List<SUP_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<SUP_APPTYPE_PROG_GROUPS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupProgramme)).DataSource.Data;
                    if (FetchProgrammeGroup != null && FetchProgrammeGroup.Count > 0)
                        objViewModel.ProgrammeGroupList = new SelectList(FetchProgrammeGroup, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_NAME);
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                    var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                    if (FetchClassYear != null && FetchClassYear.Count > 0)
                        objViewModel.ClassYearList = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeeStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult ListHostelFeeStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ListHostelFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ListHostelFeeStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);

        }
        public ActionResult ListMessFeeStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ListHostelFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ListHostelFeeStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);

        }
        public JsonResult FetchFrequencyByAcademicYear(string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;

                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "' cms-FrequencyType=" + item.FEE_MODE + ">" + item.FREQUENCY_NAME + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchFrequencyByAcademicYearForHostel(string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;
                    //var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllHostelFrequency), sAcademicYear).DataSource.Data;
                    //if (FetchFrequency != null && FetchFrequency.Count > 0)
                    //{
                    //    foreach (var item in FetchFrequency)
                    //        objResult.sResult += "<option value='" + item.FREQUENCY_ID + ">" + item.FREQUENCY_NAME + "</option>";
                    //}
                    var sql = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId).Replace((Common.Delimiter.QUS + "FEE_ROOT_ID"), "2,3,4,5");
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY(), sql, sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "' cms-FrequencyType=" + item.FEE_MODE + ">" + item.FREQUENCY_NAME + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindAdmClassesByProgrammeMode(string sProgrammeMode)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.PROGRAMME_MODE = sProgrammeMode;
                var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionClassesByProgrammeMode), sAcademicyear).DataSource.Data;
                if (FetchClass != null && FetchClass.Count > 0)
                {
                    foreach (var item in FetchClass)
                        objResult.sResult += "<option value='" + item.CLASS_ID + "'>" + item.CLASS_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindFeeStructureByFrequencyIdAndClassId(string sProgrammeGroupId, string sFrequency, string sClassYearId, string Academicyear)
        {

            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.PROGRAMME = sProgrammeGroupId;
                    objModel.FREQUENCY = sFrequency;
                    objModel.CLASS_YEAR_ID = sClassYearId;
                    objViewModel.lstFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeStructureForEditByClass), Academicyear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindFeeStructureByFrequencyIdAndClassId", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindFeeStructureByFrequencyIdAndClassId", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchHeadsByFeeCategory(string sFeeCategory)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_HEAD objModel = new SUP_HEAD();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.FEE_CATEGORY = sFeeCategory;
                var FetchHeads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadByFeeCategory), sAcademicyear).DataSource.Data;
                if (FetchHeads != null && FetchHeads.Count > 0)
                {
                    foreach (var item in FetchHeads)
                        objResult.sResult += "<option value='" + item.HEAD_ID + "'>" + item.HEAD + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchClassByProgrammeId(string sProgramme)
        {
            JsonResultData objResult = new JsonResultData();
            cp_Classes_2017 objModel = new cp_Classes_2017();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.PROGRAMME = sProgramme;
                objModel.PROGRAMME_GROUP_ID = sProgramme;
                var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByProgrammeGroupId), sAcademicyear).DataSource.Data;
                if (FetchClass != null && FetchClass.Count > 0)
                {
                    foreach (var item in FetchClass)
                        objResult.sResult += "<option value='" + item.CLASS_ID + "'>" + item.CLASS_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchFeeStructureById(string sFeeStructureId)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sFeeStructureId != null)
                {
                    string sOption = string.Empty;
                    objModel.FEE_STRUCTURE_ID = sFeeStructureId;
                    var FetchFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStructureById)).DataSource.Data;
                    return Json(FetchFeeStructure);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveOrUpdateFeeStructure(string sFeeStructure)
        {
            JsonResultData objResult = new JsonResultData();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                if (sFeeStructure != null)
                {
                    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                    {
                        var Result = serializer.Deserialize<FEE_STRUCTURE_LIST>(sFeeStructure);
                        foreach (var item in Result.FEE_STRUCTURE)
                        {
                            //var FetchProgramme = (List<CP_CLASSES>)CMSHandler.FetchData<CP_CLASSES>(item, SQL.Academics.AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassInfoById)).DataSource.Data;                          
                            if (!string.IsNullOrEmpty(item.FEE_STRUCTURE_ID))
                            {
                                if (!string.IsNullOrEmpty(item.AMOUNT))
                                {
                                    var sresultArgs = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeStructure));
                                    objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                                }
                                else
                                {
                                    var DeleteFeeStructure = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStructure));
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(item.AMOUNT))
                                {
                                    var sresultArgs = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStructure));
                                    objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                                }
                                else
                                {

                                }
                            }

                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                        objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeeStructure(string sFeeStructureId)
        {
            JsonResultData ObjResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    if (!string.IsNullOrEmpty(sFeeStructureId))
                    {
                        var objModel = new FEE_STRUCTURE();
                        objModel.FEE_STRUCTURE_ID = sFeeStructureId;
                        var sresultArgs = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStructure));
                        ObjResult.Message = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                        ObjResult.ErrorCode = Common.ErrorCode.Ok;
                    }
                    else
                    {
                        ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                        ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                    }
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", ex.Message);
                }
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrEditFeeStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                    //List<SUP_FEE_FREQUENCY> liFrequency = new List<SUP_FEE_FREQUENCY>();
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var FetchProgrammeGroup = (List<SUP_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<SUP_APPTYPE_PROG_GROUPS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupProgramme)).DataSource.Data;
                    if (FetchProgrammeGroup != null && FetchProgrammeGroup.Count > 0)
                        objViewModel.ProgrammeGroupList = new SelectList(FetchProgrammeGroup, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_NAME);
                    if (FetchClassYear != null && FetchClassYear.Count > 0)
                        objViewModel.ClassYearList = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                    //liFrequency.Add(new SUP_FEE_FREQUENCY() { FREQUENCY_ID = "0", FREQUENCY_NAME = "---Select---" });
                    //objViewModel.FrequencyList = new SelectList(liFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AddOrEditFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AddOrEditFeeStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult AddOrEditFeeHostelAndMessStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();


                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AddOrEditFeeHostelAndMessStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AddOrEditFeeHostelAndMessStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult AddOrEditFeeMessStructure()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();


                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AddOrEditFeeMessStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AddOrEditFeeMessStructure", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchHostelFrequencyByAcademicYear(string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;

                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.HostelFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "' cms-FrequencyType=" + item.FEE_MODE + ">" + item.FREQUENCY_NAME + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchHostelFrequencyByAcademicYear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchHostelFrequencyByAcademicYear", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchMessFrequencyByAcademicYear(string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;

                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.MessFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "' cms-FrequencyType=" + item.FEE_MODE + ">" + item.FREQUENCY_NAME + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchMessFrequencyByAcademicYear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchMessFrequencyByAcademicYear", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchHostelAndMessMainHeadByFrequencyId(string sFrequencyId, string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FREQUENCY_ID = sFrequencyId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainHeadsByFrequencyIdForMessAndHostel), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.MAIN_HEAD_ID + "'>" + item.MAIN_HEAD + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchHostelAndMessMainHeadByFrequencyId", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchHostelAndMessMainHeadByFrequencyId", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindFeeStructureByFrequencyIdAndClassIdForAddOrEdit(string sProgrammeGroupId, string sFrequency, string sClassYearId, string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;
                    objModel.PROGRAMME = sProgrammeGroupId;
                    objModel.FREQUENCY = sFrequency;
                    objModel.CLASS_YEAR_ID = sClassYearId;
                    objViewModel.lstFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeStructureForEditByClass), sAcademicYear).DataSource.Data;
                    objViewModel.Bank = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts), sAcademicYear).DataSource.Data;
                    objViewModel.FeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindFeeStructureByFrequencyIdAndClassIdForAddOrEdit", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindFeeStructureByFrequencyIdAndClassIdForAddOrEdit", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindHostelAndMessFeeStructureByFrequencyIdForAddOrEdit(string sFrequency, string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Academicyear;
                    objModel.FREQUENCY = sFrequency;
                    objViewModel.lstFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeHostelStructure), sAcademicYear).DataSource.Data;
                    objViewModel.Bank = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts), sAcademicYear).DataSource.Data;
                    objViewModel.FeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindHostelAndMessFeeStructureByFrequencyIdForAddOrEdit", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindHostelAndMessFeeStructureByFrequencyIdForAddOrEdit", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindFeeHosteAndMesslStructureByFrequencyId(string sFrequency, string Academicyear)
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {

                    objModel.FREQUENCY = sFrequency;
                    objViewModel.lstFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.BindFeeHostelStructure), Academicyear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindFeeHosteAndMesslStructureByFrequencyId", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindFeeHosteAndMesslStructureByFrequencyId", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveOrUpdateFeeHostelAndMessStructure(string sFeeStructure)
        {
            JsonResultData objResult = new JsonResultData();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                if (sFeeStructure != null)
                {
                    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                    {
                        var Result = serializer.Deserialize<FEE_STRUCTURE_LIST>(sFeeStructure);
                        foreach (var item in Result.FEE_STRUCTURE)
                        {
                            //var FetchProgramme = (List<CP_CLASSES>)CMSHandler.FetchData<CP_CLASSES>(item, SQL.Academics.AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassInfoById)).DataSource.Data;                          
                            if (!string.IsNullOrEmpty(item.FEE_STRUCTURE_ID))
                            {
                                if (!string.IsNullOrEmpty(item.AMOUNT))
                                {
                                    var sresultArgs = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeStructureForHostelAndMess));
                                    objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                                }
                                else
                                {
                                    var DeleteFeeStructure = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeHostelAndMessStructure));
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(item.AMOUNT))
                                {
                                    var sresultArgs = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveHostelAndMessFeeStructure));
                                    objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                                }
                                else
                                {

                                }
                            }

                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                        objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteHostelAndMessFeeStructure(string sFeeStructureId)
        {
            JsonResultData ObjResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    if (!string.IsNullOrEmpty(sFeeStructureId))
                    {
                        var objModel = new FEE_STRUCTURE();
                        objModel.FEE_STRUCTURE_ID = sFeeStructureId;
                        var sresultArgs = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeHostelAndMessStructure));
                        ObjResult.Message = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                        ObjResult.ErrorCode = Common.ErrorCode.Ok;
                    }
                    else
                    {
                        ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                        ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                    }
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("SaveOrUpdateFeeStructure", "Fee", ex.Message);
                }
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion       

        #region Fee Discount
        public ActionResult ListFeeDiscount()
        {
            FeeDiscountViewModel objViewModel = new FeeDiscountViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objViewModel.lstFeeDiscount = (List<FEE_DISCOUNT>)CMSHandler.FetchData<FEE_DISCOUNT>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscount), sAcademicYear).DataSource.Data;
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
            return View(objViewModel);
        }
        public JsonResult SaveOrUpdateFeeDiscount(string sDiscount, string sFrequency)
        {
            JsonResultData objResult = new JsonResultData();
            if (sDiscount != null)
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var Result = JsonConvert.DeserializeObject<FEE_DISCOUNT>(sDiscount);
                    if (!string.IsNullOrEmpty(Result.DISCOUNT_ID))
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(Result, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeDiscount));
                        if (sresultArgs.Success)
                        {
                            var obj = JsonConvert.DeserializeObject<List<FEE_DISCOUNT_FREQUENCY>>(sFrequency);
                            var TempList = new List<FEE_DISCOUNT_FREQUENCY>();
                            var FetchFeeDiscountFrequency = (List<FEE_DISCOUNT_FREQUENCY>)CMSHandler.FetchData<FEE_DISCOUNT_FREQUENCY>(Result, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountFrequencyByDiscountId)).DataSource.Data;
                            //this loop removes existing main heads 
                            if (FetchFeeDiscountFrequency != null && FetchFeeDiscountFrequency.Count() > 0)
                            {
                                if (obj != null && obj.Count > 0)
                                {
                                    foreach (var list in FetchFeeDiscountFrequency)
                                    {

                                        TempList.Add(obj.SingleOrDefault(S => S.DISCOUNT_ID == list.DISCOUNT_ID && S.FREQUENCY_ID == list.FREQUENCY_ID));
                                        obj.Remove(obj.SingleOrDefault(S => S.DISCOUNT_ID == list.DISCOUNT_ID && S.FREQUENCY_ID == list.FREQUENCY_ID));
                                    }
                                    foreach (var item in TempList)
                                    {
                                        if (item != null)
                                            FetchFeeDiscountFrequency.Remove(FetchFeeDiscountFrequency.SingleOrDefault(S => S.DISCOUNT_ID == item.DISCOUNT_ID && S.FREQUENCY_ID == item.FREQUENCY_ID));
                                    }
                                    //this loop deletes records
                                    foreach (var item in FetchFeeDiscountFrequency)
                                    {
                                        var o = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountFrequency));
                                    }
                                }
                            }
                            //this loop saves records
                            foreach (var item in obj)
                            {
                                item.DISCOUNT_ID = Result.DISCOUNT_ID;
                                var SaveFeeFrequency = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeDiscountFrequency));
                                if (SaveFeeFrequency.Success)
                                {
                                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(Result, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeDiscount), null, true);
                        if (sresultArgs.Success)
                        {
                            var s = sresultArgs.RowUniqueId.ToString();
                            var obj = JsonConvert.DeserializeObject<List<FEE_DISCOUNT_FREQUENCY>>(sFrequency);
                            foreach (var item in obj)
                            {
                                item.DISCOUNT_ID = sresultArgs.RowUniqueId.ToString();
                                var SaveFeeDiscountFrequency = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeDiscountFrequency));
                                objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                                objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.BadRequest : Common.ErrorCode.BadRequest;
                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeeDiscount(string sDiscountId)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sDiscountId))
                {
                    var objModel = new FEE_DISCOUNT();
                    objModel.DISCOUNT_ID = sDiscountId;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscount));
                    if (sresultArgs.Success)
                    {
                        var sresultArg = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountFrequency));
                        if (sresultArg.Success)
                        {
                            var resultArg = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountHeadByDiscountId));
                            ObjResult.sResult = (resultArg.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                        }
                        else
                        {
                            ObjResult.sResult = Common.Messages.FailedToDeletedTryAgain;
                            ObjResult.ErrorCode = Common.ErrorCode.Ok;
                            ObjResult.Message = Common.ErrorMessage.Ok;
                        }
                    }
                    else
                    {
                        ObjResult.sResult = Common.Messages.FailedToDeletedTryAgain;
                        ObjResult.ErrorCode = Common.ErrorCode.Ok;
                        ObjResult.Message = Common.ErrorMessage.Ok;
                    }
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                }
            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchFeeDiscountById(string sDiscountId)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_DISCOUNT objModel = new FEE_DISCOUNT();
            if (sDiscountId != null)
            {
                string sOption = string.Empty;
                objModel.DISCOUNT_ID = sDiscountId;
                var FetchFrequency = (List<FEE_DISCOUNT>)CMSHandler.FetchData<FEE_DISCOUNT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountById)).DataSource.Data;
                return Json(FetchFrequency);
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FetchFrequecyByType(string sFrequencyType, string sDiscountId)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_FEE_FREQUENCY objModel = new SUP_FEE_FREQUENCY();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.TYPE = sFrequencyType;
                objModel.DISCOUNT_ID = sDiscountId;
                if (!string.IsNullOrEmpty(sDiscountId))
                {
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFrequencyByTypeAndDiscountId), sAcademicyear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "' " + item.STATUS + ">" + item.FREQUENCY_NAME + "</option>";
                    }
                }
                else
                {
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFrequencyByType), sAcademicyear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        foreach (var item in FetchFrequency)
                            objResult.sResult += "<option value='" + item.FREQUENCY_ID + "'>" + item.FREQUENCY_NAME + "</option>";
                    }
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Fee disocunt Allotment
        public ActionResult FeeDiscountAllotment()
        {
            FeeDiscountAllotmentviewModel objViewModel = new FeeDiscountAllotmentviewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicyear).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeeDiscountAllotment", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeeDiscountAllotment", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindFeeDiscountAllotmentByClassIdAndAcademicyear(string sClassId, string AcademicYear)
        {
            FeeDiscountAllotmentviewModel objViewModel = new FeeDiscountAllotmentviewModel();
            FEE_DISCOUNT_ALLOTMENT objModel = new FEE_DISCOUNT_ALLOTMENT();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.ACCADEMIC_YEAR = AcademicYear;
                    objModel.CLASS_ID = sClassId;
                    objViewModel.lstFeeDiscountAllotment = (List<FEE_DISCOUNT_ALLOTMENT>)CMSHandler.FetchData<FEE_DISCOUNT_ALLOTMENT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountAllotmentByClassIdAndAcademicyear)).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindFeeDiscountAllotmentByClassIdAndAcademicyear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindFeeDiscountAllotmentByClassIdAndAcademicyear", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchAcademicYearAndClassAndDiscount(string sClassId)
        {
            JsonResultData objResult = new JsonResultData();
            Student_Personal_Info objModel = new Student_Personal_Info();
            string Academicyear = string.Empty;
            string sClass = string.Empty;
            string sDiscount = string.Empty;
            string sFeeCategory = string.Empty;
            string sFrequency = string.Empty;
            string sHead = string.Empty;
            string sProgramme = string.Empty;
            string sStudent = string.Empty;
            string JsonData = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.CLASS = sClassId;
                //var liClass = new List<cp_Classes_2017>();
                //liClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicyear).DataSource.Data;
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                var FeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                //var FetchProgramme = (List<cp_Program_2017>)CMSHandler.FetchData<cp_Program_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicyear).DataSource.Data;
                var FetchDiscount = (List<FEE_DISCOUNT>)CMSHandler.FetchData<FEE_DISCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDiscount)).DataSource.Data;
                var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllFrequency), sAcademicyear).DataSource.Data;
                var FetchHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHead), sAcademicyear).DataSource.Data;
                var FetchStudent = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByClassID), sAcademicyear).DataSource.Data;
                //if (liClass != null && liClass.Count > 0)
                //{
                //    foreach (var item in liClass)
                //        sClass += "<option value='" + item.CLASS_ID + "'>" + item.CLASS_NAME + "</option>";
                //}
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                {
                    foreach (var item in FetchAcademicYear)
                        Academicyear += "<option value='" + item.ACADEMIC_YEAR_ID + "'>" + item.AC_YEAR + "</option>";
                }
                if (FetchDiscount != null && FetchDiscount.Count > 0)
                {
                    foreach (var item in FetchDiscount)
                        sDiscount += "<option value='" + item.DISCOUNT_ID + "'>" + item.DISCOUNT_NAME + "</option>";
                }
                if (FeeCategory != null && FeeCategory.Count > 0)
                {
                    foreach (var item in FeeCategory)
                        sFeeCategory += "<option value='" + item.FEE_CATEGORY_ID + "'>" + item.FEE_CATEGORY + "</option>";
                }
                //if (FetchProgramme != null && FetchProgramme.Count > 0)
                //{
                //    foreach (var item in FetchProgramme)
                //        sProgramme += "<option value='" + item.PROGRAMME_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                //}
                if (FetchFrequency != null && FetchFrequency.Count > 0)
                {
                    foreach (var item in FetchFrequency)
                        sFrequency += "<option value='" + item.FREQUENCY_ID + "'>" + item.FREQUENCY_NAME + "</option>";
                }
                if (FetchHead != null && FetchHead.Count > 0)
                {
                    foreach (var item in FetchHead)
                        sHead += "<option value='" + item.HEAD_ID + "'>" + item.HEAD + "</option>";
                }
                if (FetchStudent != null && FetchStudent.Count > 0)
                {
                    foreach (var item in FetchStudent)
                        sHead += "<option value='" + item.STUDENT_ID + "'>" + item.FIRST_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            var objJsonData = new FEE_DISCOUNT_ALLOTMENT() { DISCOUNT_ID = sDiscount, CLASS_ID = sClass, ACCADEMIC_YEAR = Academicyear, PROGRAMME = sProgramme, FREQUENCY = sFrequency, FEE_CATEGORY = sFeeCategory, HEAD = sHead, STUDENT_ID = sStudent };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchStudentByClassId(string sClassId, string sDiscountId)
        {
            JsonResultData objResult = new JsonResultData();
            cp_Classes_2017 objModel = new cp_Classes_2017();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.CLASS_ID = sClassId;
                objModel.DISCOUNT_ID = sDiscountId;
                var FetchStudent = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfoByClassIdAndDiscountId), sAcademicyear).DataSource.Data;
                if (FetchStudent != null && FetchStudent.Count > 0)
                {
                    foreach (var item in FetchStudent)
                        objResult.sResult += "<option value='" + item.STUDENT_ID + "' " + item.STATUS + ">" + item.FIRST_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDiscountAllotment(string sDiscountAllotment)
        {
            JsonResultData objResult = new JsonResultData();
            if (sDiscountAllotment != null)
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    FEE_DISCOUNT_ALLOTMENT objModel = new FEE_DISCOUNT_ALLOTMENT();
                    var TempList = new List<FEE_DISCOUNT_ALLOTMENT>();
                    //string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var obj = JsonConvert.DeserializeObject<List<FEE_DISCOUNT_ALLOTMENT>>(sDiscountAllotment);
                    foreach (var item in obj)
                    {
                        objModel.DISCOUNT_ID = item.DISCOUNT_ID;
                        objModel.CLASS_ID = item.CLASS_ID;
                    }
                    objModel.ACCADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFeeDiscountAllotmentByDiscountId = (List<FEE_DISCOUNT_ALLOTMENT>)CMSHandler.FetchData<FEE_DISCOUNT_ALLOTMENT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountByClassIdAndDiscountId)).DataSource.Data;
                    //this loop removes existing main heads 
                    if (FetchFeeDiscountAllotmentByDiscountId != null && FetchFeeDiscountAllotmentByDiscountId.Count() > 0)
                    {
                        if (obj != null && obj.Count > 0)
                        {
                            foreach (var list in FetchFeeDiscountAllotmentByDiscountId)
                            {
                                TempList.Add(obj.SingleOrDefault(S => S.STUDENT_ID == list.STUDENT_ID && S.DISCOUNT_ID == list.DISCOUNT_ID && S.CLASS_ID == list.CLASS_ID));
                                obj.Remove(obj.SingleOrDefault(S => S.STUDENT_ID == list.STUDENT_ID && S.DISCOUNT_ID == list.DISCOUNT_ID && S.CLASS_ID == list.CLASS_ID));
                            }
                            foreach (var item in TempList)
                            {
                                if (item != null)
                                    FetchFeeDiscountAllotmentByDiscountId.Remove(FetchFeeDiscountAllotmentByDiscountId.SingleOrDefault(S => S.DISCOUNT_ID == item.DISCOUNT_ID && S.CLASS_ID == item.CLASS_ID && S.STUDENT_ID == item.STUDENT_ID));
                            }
                            //this loop deletes records
                            foreach (var item in FetchFeeDiscountAllotmentByDiscountId)
                            {
                                item.ACCADEMIC_YEAR = objModel.ACCADEMIC_YEAR;
                                var o = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountAllotment));

                            }
                        }
                    }
                    //this loop saves records
                    foreach (var item in obj)
                    {
                        item.ACCADEMIC_YEAR = objModel.ACCADEMIC_YEAR;
                        var SaveFeeDiscountAllotment = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeDiscountAllotment));
                        if (SaveFeeDiscountAllotment.Success)
                        {
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeeDiscountAllotment(string sDiscountAllotmentId)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sDiscountAllotmentId))
                {
                    var objModel = new FEE_DISCOUNT_ALLOTMENT();
                    objModel.DISCOUNT_ALLOTMENT_ID = sDiscountAllotmentId;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountAllotment));
                    ObjResult.Message = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                    ObjResult.ErrorCode = Common.ErrorCode.Ok;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                }
            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Fee Discount Head
        public ActionResult ListFeeDiscountHead()
        {
            FeeDiscountHeadViewModel objViewModel = new FeeDiscountHeadViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objViewModel.lstFeeDiscountHead = (List<FEE_DISCOUNT_HEAD>)CMSHandler.FetchData<FEE_DISCOUNT_HEAD>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountHead)).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ListFeeDiscountHead", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ListFeeDiscountHead", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchFeeFrequencyByDiscountId(string sDiscountId)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_DISCOUNT_FREQUENCY objModel = new FEE_DISCOUNT_FREQUENCY();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.DISCOUNT_ID = sDiscountId;
                var FetchFeeFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFrequencyByDiscountId), sAcademicyear).DataSource.Data;
                if (FetchFeeFrequency != null && FetchFeeFrequency.Count > 0)
                {
                    objResult.sResult = "<option value='0'>--Select--</option>";
                    foreach (var item in FetchFeeFrequency)
                        objResult.sResult += "<option value='" + item.FREQUENCY_ID + "'>" + item.FREQUENCY_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchHeadByFrequencyIdAndDiscountId(string sFrequencyId, string sDiscountId)
        {
            JsonResultData objResult = new JsonResultData();
            FEE_DISCOUNT_FREQUENCY objModel = new FEE_DISCOUNT_FREQUENCY();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                objModel.FREQUENCY_ID = sFrequencyId;
                objModel.DISCOUNT_ID = sDiscountId;
                var FetchHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchHeadByDiscountIdAndFrequencyId), sAcademicyear).DataSource.Data;
                if (FetchHead != null && FetchHead.Count > 0)
                {
                    foreach (var item in FetchHead)
                        objResult.sResult += "<option value='" + item.HEAD_ID + "' " + item.STATUS + ">" + item.HEAD + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFeeDiscountHead(string sDiscountHead)
        {
            JsonResultData objResult = new JsonResultData();
            if (sDiscountHead != null)
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    FEE_DISCOUNT_HEAD objModel = new FEE_DISCOUNT_HEAD();
                    var TempList = new List<FEE_DISCOUNT_HEAD>();
                    //string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var obj = JsonConvert.DeserializeObject<List<FEE_DISCOUNT_HEAD>>(sDiscountHead);
                    foreach (var item in obj)
                    {
                        objModel.DISCOUNT_ID = item.DISCOUNT_ID;
                        objModel.FREQUENCY_ID = item.FREQUENCY_ID;
                    }
                    //objModel.ACCADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFeeDiscountAllotmentByDiscountId = (List<FEE_DISCOUNT_HEAD>)CMSHandler.FetchData<FEE_DISCOUNT_HEAD>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeDiscountHeadByDiscountIdAndFrequencyId)).DataSource.Data;
                    //this loop removes existing main heads 
                    if (FetchFeeDiscountAllotmentByDiscountId != null && FetchFeeDiscountAllotmentByDiscountId.Count() > 0)
                    {
                        if (obj != null && obj.Count > 0)
                        {
                            foreach (var list in FetchFeeDiscountAllotmentByDiscountId)
                            {
                                TempList.Add(obj.SingleOrDefault(S => S.HEAD_ID == list.HEAD_ID && S.DISCOUNT_ID == list.DISCOUNT_ID && S.FREQUENCY_ID == list.FREQUENCY_ID));
                                obj.Remove(obj.SingleOrDefault(S => S.HEAD_ID == list.HEAD_ID && S.DISCOUNT_ID == list.DISCOUNT_ID && S.FREQUENCY_ID == list.FREQUENCY_ID));
                            }
                            foreach (var item in TempList)
                            {
                                if (item != null)
                                    FetchFeeDiscountAllotmentByDiscountId.Remove(FetchFeeDiscountAllotmentByDiscountId.SingleOrDefault(S => S.DISCOUNT_ID == item.DISCOUNT_ID && S.HEAD_ID == item.HEAD_ID && S.FREQUENCY_ID == item.FREQUENCY_ID));
                            }
                            //this loop deletes records
                            foreach (var item in FetchFeeDiscountAllotmentByDiscountId)
                            {
                                //item.ACCADEMIC_YEAR = objModel.ACCADEMIC_YEAR;
                                var o = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountHead));

                            }
                        }
                    }
                    //this loop saves records
                    foreach (var item in obj)
                    {
                        //item.ACCADEMIC_YEAR = objModel.ACCADEMIC_YEAR;
                        var SaveFeeDiscountAllotment = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeDiscountHead));
                        if (SaveFeeDiscountAllotment.Success)
                        {
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFeeDiscountHead(string sDiscountHead)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sDiscountHead))
                {
                    var objModel = new FEE_DISCOUNT_HEAD();
                    objModel.DISCOUNT_HEAD_ID = sDiscountHead;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeDiscountHead));
                    ObjResult.Message = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                    ObjResult.ErrorCode = Common.ErrorCode.Ok;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                }
            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Advance
        public ActionResult FeeAdavancePayment()
        {
            FeeAdvancePaymentViewModel objViewModel = new FeeAdvancePaymentViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
                if (FetchClass != null && FetchClass.Count > 0)
                    objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
            return View(objViewModel);
        }
        #endregion
        #region Student Account Credit Insert

        #endregion

        #region PayuStatus
        // PayUStatus Main page ...
        [UserRights("Admin")]
        public ActionResult PayuStatus()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    var ObjViewModel = new FeePayUStatusViewModel();
                    var liApplicationtype = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (liApplicationtype != null && liApplicationtype.Count > 0)
                    {
                        ObjViewModel.Application_Type = new SelectList(liApplicationtype, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                    }
                    var lishift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    if (lishift != null && lishift.Count != 0)
                    {
                        ObjViewModel.Shift = new SelectList(lishift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    }
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("PayuStatus", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("PayuStatus", "Fee", ex.Message);
                    }
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }

        }

        // partial Bind PayUStatus By Application TypeId And ProgramID ...
        public ActionResult ListPayUStatus(string sApplicationTypeId, string sProgrammeId)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sApplicationTypeId) && !string.IsNullOrEmpty(sProgrammeId))
                {
                    try
                    {
                        var ObjViewModel = new FeePayUStatusViewModel();
                        var ObjTransferProgram = new PayUStatus();
                        ObjTransferProgram.APPLICATION_TYPE_ID = sApplicationTypeId;
                        ObjTransferProgram.PROGRAMME_GROUP_ID = sProgrammeId;
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
                        string sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransferProgramList).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sProgrammeId);
                        ObjViewModel.liPayUStatus = (List<PayUStatus>)CMSHandler.FetchData<PayUStatus>(ObjTransferProgram, sSql, sAcademicYear).DataSource.Data;
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("ListTransferProgram", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("ListTransferProgram", "Fee", ex.Message);
                        }
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }
        }

        // This Action Is Used To Get Response From PayU And Show Details ....
        public ActionResult PayUResponseByTranscationId(string sTransactionId, string IssueId)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sTransactionId) && !string.IsNullOrEmpty(IssueId))
                {
                    try
                    {
                        var ObjViewModel = new FeePayUStatusViewModel();
                        string Ac_Year = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        Initiate(sTransactionId, Ac_Year);
                        ObjViewModel.liPayUResponse = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(new fee_payu_response() { txnid = sTransactionId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayURequestByTransactionId), Ac_Year).DataSource.Data;
                        var liApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = IssueId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentApplicationNoByIssueId)).DataSource.Data;
                        if (liApplication != null && liApplication.Count > 0)
                            ObjViewModel.Application_No = liApplication.FirstOrDefault().APPLICATION_NO;
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("GetPayUResponseByTranscationId", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("GetPayUResponseByTranscationId", "Fee", ex.Message);
                        }
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
        #endregion

        #region PayUTransactionUpdates
        public static void Initiate(string sTransactionId, string sAcademicYear)
        {
            try
            {

                string[] hashSquence;
                string txids = string.Empty;
                string hash = string.Empty;

                JsonResultData objresultdata = new JsonResultData();
                // var result = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SQL.Academics.AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchAcademicYearInfo)).DataSource.Data;
                string AcademicYear = (Common.SESSION_VARIABLES.ACADEMIC_YEAR != null) ? Common.SESSION_VARIABLES.ACADEMIC_YEAR.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    var APIs = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionApplicationFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUAPI)).DataSource.Data;
                    if (APIs != null && APIs.Count > 0)
                    {
                        //var ObjResponse = new fee_payu_response();
                        //ObjResponse.txnid = sTransactionId;
                        var request = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(new fee_payu_response() { txnid = sTransactionId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayURequestByTransactionId), sAcademicYear).DataSource.Data;
                        if (request != null && request.Count > 0)
                        {
                            foreach (var item in APIs)
                            {
                                hash = GetHashValueForUpdates(item.HASH_SEQUENCE, item.KEY, sTransactionId, item.curl, item.SALT);
                                var value = new Dictionary<string, string>
                                              {
                                                 { "key", item.KEY },
                                                 { "command", "verify_payment" },
                                                 { "hash", hash },
                                                 { "var1", sTransactionId },
                                               };

                                var apiResponse = CommonMethods.HttpPostMethod(item.BASE_URL, value);
                                JsonArrayAttribute objJsonArray = new JsonArrayAttribute();
                                JsonObjectAttribute jsn = new JsonObjectAttribute();
                                if (apiResponse != null)
                                {
                                    var transactionObject = JObject.Parse(apiResponse.ToString()) as JObject;
                                    var valuesjson = transactionObject.Root["transaction_details"];
                                    try
                                    {
                                        var responseItem = JsonConvert.DeserializeObject<fee_payu_response>(valuesjson[sTransactionId].ToString());
                                        if (string.IsNullOrEmpty(responseItem.amount))
                                            responseItem.amount = responseItem.transaction_amount;
                                        if (responseItem != null && !string.IsNullOrEmpty(responseItem.txnid))
                                        {
                                            responseItem.PAYU_RESPONSE_ID = request.FirstOrDefault().PAYU_RESPONSE_ID;
                                            var responseResult = CMSHandler.SaveOrUpdate(responseItem, string.IsNullOrEmpty(request.FirstOrDefault().PAYU_RESPONSE_ID) ? FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayUResponse) : FeeSQL.GetFeeSQL(FeeSQLCommands.UpdatePayUResponse), sAcademicYear);
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        using (ErrorLog objlog = new ErrorLog())
                                        {
                                            var LineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                                            objlog.WriteError("", "fee_payU_response", "Failed" + "|" + LineNumber + "|" + ex.Message);
                                        }

                                    }
                                }
                            }

                            //call fetch method and update the fee collection . 

                            //var paymentUpdates = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUResponseForUpdate), result.FirstOrDefault().AC_YEAR).DataSource.Data;
                            //if (paymentUpdates != null && paymentUpdates.Count > 0)
                            //{
                            //    foreach (var item in paymentUpdates)
                            //    {
                            //        objresultdata = UpdatePayments(result.FirstOrDefault().AC_YEAR, item.udf2, item.PAYU_RESPONSE_ID, item.udf1);
                            //        using (ErrorLog objlog = new ErrorLog())
                            //        {
                            //            objlog.WriteError(item.txnid, "Application fee transaction", objresultdata.Message + "|" + item.udf1);
                            //        }
                            //        if (objresultdata.ErrorCode == "100")
                            //        {
                            //            CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayUDoubleEntryResponse), result.FirstOrDefault().AC_YEAR);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objlog = new ErrorLog())
                {
                    var LineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                    objlog.WriteError(LineNumber.ToString(), "fee_payU_response", "Failed" + "|" + ex.Message);
                }
            }

        }

        private static string GetHashValueForUpdates(string hashSeq, string sMerchant_key, string txids, string sCommand, string sSalt)
        {
            string hash_string = string.Empty;
            string hash = string.Empty;
            string[] hashVarsSeq = hashSeq.Split('|'); // spliting hash sequence from config       
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + sMerchant_key;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "command")
                {
                    hash_string = hash_string + sCommand;
                    hash_string = hash_string + '|';
                }


            }
            hash_string = hash_string + txids;
            hash_string = hash_string + '|';
            hash_string += sSalt;// appending SALT

            hash = CommonMethods.Generatehash512(hash_string).ToLower();         //generating hash
            return hash;
        }
        #endregion

        #region AdmissionFee
        //public ActionResult AdmissionFeeStucture()
        //{

        //    AdmissionFeeViewModel admissionViewModel = new AdmissionFeeViewModel();
        //    try
        //    {
        //        if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
        //        {
        //            var programmeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
        //            if (programmeMode != null && programmeMode.Count > 0)
        //            {
        //                admissionViewModel.programmeMode = new SelectList(programmeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
        //            }


        //        }
        //        else
        //            return RedirectToAction("ErrorMessage", "error");
        //    }
        //    catch (Exception ex)
        //    {
        //        using (ErrorLog objHandler = new ErrorLog())
        //        {
        //            objHandler.WriteError("AdmissionFeeStucture", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
        //            objHandler.WriteError("AdmissionFeeStucture", "Fee", ex.Message);
        //        }
        //    }
        //    return View(admissionViewModel);
        //    //   admissionViewModel.programmeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, CMS.SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchProgrammeListByShiftId), (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty).DataSource.Data;

        //}

        //[UserRights("Applicant")]
        //public ActionResult PayAdmissionFee()
        //{
        //    List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();

        //    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
        //    {
        //        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
        //        if (FetchFrequency != null && FetchFrequency.Count > 0)
        //            studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //    }
        //    return View(studentAcountList);
        //}
        //public ActionResult PaymentForAdmissionFee()
        //{

        //    PayUPost objPayUPost = new PayUPost();
        //    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
        //    {
        //        var objFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //        if (objFrequency!=null && objFrequency.Count>0)
        //        {
        //            var objResult = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = objFrequency.FirstOrDefault().FREQUENCY_ID, STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByUserID), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;

        //            if (objResult != null && objResult.Count > 0)
        //            {
        //                var person = objResult.FirstOrDefault();
        //                var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;
        //                if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
        //                {
        //                    objPayUPost.productinfo = "Admission Fee";
        //                    objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
        //                    objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
        //                    objPayUPost.udf1 = person.STUDENT_ID;
        //                    objPayUPost.email = person.EMAIL_ID;
        //                    objPayUPost.udf5 = person.CMOBILENO;
        //                    objPayUPost.firstname = person.FIRST_NAME;
        //                    objPayUPost.udf3 = person.FREQUENCY_ID;
        //                    objPayUPost.udf4 = person.APPLICATION_NO;
        //                    objPayUPost.udf2 = Common.FrequencyType.AdmissionFee;
        //                    objPayUPost.txnid = CommonMethods.CommonTransactionId(person.STUDENT_ID);
        //                    objPayUPost.amount = Convert.ToDecimal(objResult.Sum(o => Convert.ToDecimal(o.BALANCE))).ToString("g29");

        //                    bool sLoop = true;
        //                    while (sLoop)
        //                    {
        //                        var fresult = (List<FEE_PAYU_REQUEST>)CMSHandler.FetchData<FEE_PAYU_REQUEST>(new FEE_PAYU_REQUEST() { txnid = objPayUPost.txnid }, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayURequestExist), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //                        if (fresult != null && fresult.Count > 0)
        //                        {
        //                            objPayUPost.txnid = CommonMethods.CommonTransactionId(person.STUDENT_ID);
        //                        }
        //                        else
        //                        {
        //                            var result = CMSHandler.SaveOrUpdate(objPayUPost, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayURequest), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
        //                            if (result.Success)
        //                                sLoop = false;
        //                        }
        //                    }
        //                    objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);
        //                    objPayUPost.surl = payUPaymentInfo.FirstOrDefault().surl;
        //                    objPayUPost.furl = payUPaymentInfo.FirstOrDefault().furl;
        //                    objPayUPost.curl = payUPaymentInfo.FirstOrDefault().curl;
        //                    objPayUPost.phone = person.CMOBILENO;
        //                    objPayUPost.BASE_URL = payUPaymentInfo.FirstOrDefault().BASE_URL.Trim();
        //                    return View(objPayUPost);
        //                }
        //            }
        //            else
        //            {
        //                objPayUPost.sMessage = "Sorry for the inconvenience,Please Close your browser and register again. ";
        //            }
        //        }
        //        else
        //        {
        //            objPayUPost.sMessage = "Sorry for the inconvenience,Account is not available. ";
        //        }


        //    }
        //    else
        //    {
        //        objPayUPost.sMessage = "Sorry for the inconvenience,Time out error , Please Close your browser and register again.";
        //    }
        //    return View(objPayUPost);
        //}
        #endregion
        #region Hostel Fee Payment 
        //[UserRights("Applicant")]
        //public ActionResult PayHostelFee()
        //{
        //    List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();

        //    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
        //    {
        //        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
        //        if (FetchFrequency != null && FetchFrequency.Count > 0)
        //            studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString(), FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //    }
        //    return View(studentAcountList);
        //}
        //public ActionResult PaymentForHostelFee()
        //{

        //    PayUPost objPayUPost = new PayUPost();
        //    if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
        //    {
        //        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
        //        string s = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //        var objFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //        if (objFrequency != null && objFrequency.Count > 0)
        //        {
        //            var objResult = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = objFrequency.FirstOrDefault().FREQUENCY_ID, STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByUserID), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;

        //            if (objResult != null && objResult.Count > 0)
        //            {
        //                var person = objResult.FirstOrDefault();
        //                var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;
        //                if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
        //                {
        //                    objPayUPost.productinfo = "Hostel Fee";
        //                    objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
        //                    objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
        //                    objPayUPost.udf1 = person.STUDENT_ID;
        //                    objPayUPost.email = person.EMAIL_ID;
        //                    objPayUPost.udf5 = person.CMOBILENO;
        //                    objPayUPost.firstname = person.FIRST_NAME;
        //                    objPayUPost.udf3 = person.FREQUENCY_ID;
        //                    objPayUPost.udf4 = person.APPLICATION_NO;
        //                    objPayUPost.udf2 = Common.FrequencyType.HostelFee;
        //                    objPayUPost.txnid = CommonMethods.CommonTransactionId(person.STUDENT_ID);
        //                    objPayUPost.amount = Convert.ToDecimal(objResult.Sum(o => Convert.ToDecimal(o.BALANCE))).ToString("g29");
        //                    // objPayUPost.liStuAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() {STUDENT_ID= person.STUDENT_ID },SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHeadByReceiveId), sAcademicYear).DataSource.Data;
        //                    bool sLoop = true;
        //                    while (sLoop)
        //                    {
        //                        var fresult = (List<FEE_PAYU_REQUEST>)CMSHandler.FetchData<FEE_PAYU_REQUEST>(new FEE_PAYU_REQUEST() { txnid = objPayUPost.txnid }, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayURequestExist), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
        //                        if (fresult != null && fresult.Count > 0)
        //                        {
        //                            objPayUPost.txnid = CommonMethods.CommonTransactionId(person.STUDENT_ID);
        //                        }
        //                        else
        //                        {
        //                            var result = CMSHandler.SaveOrUpdate(objPayUPost, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayURequest), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
        //                            if (result.Success)
        //                                sLoop = false;
        //                        }
        //                    }
        //                    objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);
        //                    objPayUPost.surl = payUPaymentInfo.FirstOrDefault().surl;
        //                    objPayUPost.furl = payUPaymentInfo.FirstOrDefault().furl;
        //                    objPayUPost.curl = payUPaymentInfo.FirstOrDefault().curl;
        //                    objPayUPost.phone = person.CMOBILENO;
        //                    objPayUPost.BASE_URL = payUPaymentInfo.FirstOrDefault().BASE_URL.Trim();
        //                    return View(objPayUPost);
        //                }
        //            }
        //            else
        //            {
        //                objPayUPost.sMessage = "Sorry for the inconvenience,Please Close your browser and register again. ";
        //            }
        //        }
        //        else
        //        {
        //            objPayUPost.sMessage = "Sorry for the inconvenience,Account is not available. ";
        //        }
        //    }
        //    else
        //    {
        //        objPayUPost.sMessage = "Sorry for the inconvenience,Time out error , Please Close your browser and register again.";
        //    }
        //    return View(objPayUPost);
        //}
        #endregion

        #region Admission Fee Receipt
        //[UserRights("APPLICANT")]
        //public ActionResult AdmissionFeeReceipt()
        //{
        //    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
        //    {
        //        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //        var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
        //        var objFeeReceipt= new FEE_RECEIPT_GENERATION();
        //        FeeTransactionViewModel objModel = new FeeTransactionViewModel();
        //        var LstfeeTransaction = new List<FEE_TRANSACTION>();
        //        var objStudentIssueModel = new ADM_ISSUE_APPLICATION_2018();
        //        try
        //        {
        //            objStudentIssueModel.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //            var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
        //            objFeeReceipt.FREQUENCY = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
        //            objFeeReceipt.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //            objFeeReceipt.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //            objModel.lstfeeAdmissionFeeGeneration= (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeReceipt, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByIssueId), sAcademicYear).DataSource.Data;
        //            objModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
        //            if (objModel.lstfeeAdmissionFeeGeneration != null && objModel.lstfeeAdmissionFeeGeneration.Count > 0)
        //            {
        //                int COUNT = 0;
        //                foreach (var item in objModel.lstfeeAdmissionFeeGeneration)
        //                {
        //                    objFeeStudentAccountModel.TRANSACTION_ID = item.TRANSACTION_ID;
        //                    objFeeStudentAccountModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
        //                    objFeeStudentAccountModel.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //                    LstfeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeReceiptByTransactionId),sAcademicYear).DataSource.Data;
        //                    if (LstfeeTransaction != null && LstfeeTransaction.Count>0)
        //                    {
        //                        if (COUNT==0)
        //                        {
        //                            objModel.liFeeTransaction = LstfeeTransaction;
        //                        }
        //                        else
        //                        {
        //                            objModel.liFeeTransaction = objModel.liFeeTransaction.Concat(LstfeeTransaction).ToList();
        //                        }                               
        //                        COUNT++;
        //                    }                           
        //                }
        //            }else
        //            {
        //                ObjJsonData.Message = Common.ErrorMessage.BadRequest;
        //                ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
        //            }
        //            return View(objModel);
        //        }
        //        catch (Exception ex)
        //        {
        //            using (ErrorLog objHandler = new ErrorLog())
        //            {
        //                objHandler.WriteError("AdmissionController", "AdmissionFeeReceipt", "Err MSg: " + ex.Message, "Query is empty!");
        //                objHandler.WriteError("AdmissionController", "AdmissionFeeReceipt", ex.Message);
        //            }
        //            return View(objModel);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("ErrorMessage", "Error");
        //    }
        //}
        //[UserRights("APPLICANT")]
        //public ActionResult HostelFeeReceipt()
        //{
        //    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
        //    {
        //        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //        var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
        //        var objFeeReceipt = new FEE_RECEIPT_GENERATION();
        //        FeeTransactionViewModel objModel = new FeeTransactionViewModel();
        //        var LstfeeTransaction = new List<FEE_TRANSACTION>();
        //        var objStudentIssueModel = new ADM_ISSUE_APPLICATION_2018();
        //        try
        //        {
        //            objStudentIssueModel.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //            var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
        //            objFeeReceipt.FREQUENCY = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
        //            objFeeReceipt.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //            objFeeReceipt.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //            objModel.lstfeeAdmissionFeeGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeReceipt, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByIssueId), sAcademicYear).DataSource.Data;
        //            objModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
        //            if (objModel.lstfeeAdmissionFeeGeneration != null && objModel.lstfeeAdmissionFeeGeneration.Count > 0)
        //            {
        //                int COUNT = 0;
        //                foreach (var item in objModel.lstfeeAdmissionFeeGeneration)
        //                {
        //                    objFeeStudentAccountModel.TRANSACTION_ID = item.TRANSACTION_ID;
        //                    objFeeStudentAccountModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
        //                    objFeeStudentAccountModel.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //                    LstfeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeReceiptByTransactionId),sAcademicYear).DataSource.Data;
        //                    if (LstfeeTransaction != null && LstfeeTransaction.Count > 0)
        //                    {
        //                        if (COUNT == 0)
        //                        {
        //                            objModel.liFeeTransaction = LstfeeTransaction;
        //                        }
        //                        else
        //                        {
        //                            objModel.liFeeTransaction = objModel.liFeeTransaction.Concat(LstfeeTransaction).ToList();
        //                        }
        //                        COUNT++;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ObjJsonData.Message = Common.ErrorMessage.BadRequest;
        //                ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
        //            }
        //            return View(objModel);
        //        }
        //        catch (Exception ex)
        //        {
        //            using (ErrorLog objHandler = new ErrorLog())
        //            {
        //                objHandler.WriteError("AdmissionController", "HostelFeeReceipt", "Err MSg: " + ex.Message, "Query is empty!");
        //                objHandler.WriteError("AdmissionController", "HostelFeeReceipt", ex.Message);
        //            }
        //            return View(objModel);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("ErrorMessage", "Error");
        //    }

        //}
        #endregion

        #region Day Wise Collection     
        public ActionResult DayWiseFeeCollectionStatus()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeFrequency), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "DayWiseFeeCollectionStatus", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "DayWiseFeeCollectionStatus", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindDayWiseFeeDetails(string sDateFrom, string sDateTo, string sFrequency)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            string sSql = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
                    objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
                    objFeeStudentAccountModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchDayWiseStudentDetailsByUserId).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY, sFrequency);
                    var FetchClassWiseFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, sSql, sAcademicYear).DataSource.Data;
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentFeeDetailsOnlyMonthByUserId).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY, sFrequency);
                    var FetchMonth = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, sSql, sAcademicYear).DataSource.Data;
                    if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                        objViewModel.liFeeTransaction = FetchClassWiseFeeDetails;
                    if (FetchMonth != null && FetchMonth.Count > 0)
                        objViewModel.liMonth = FetchMonth;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "BindDayWiseFeeDetails", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "BindDayWiseFeeDetails", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        #endregion
        #region Day Wise Fee Collection Principal View
        public ActionResult DayWiseFeeCollectionPrincipalView()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            try
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeFrequency), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "DayWiseFeeCollectionPrincipalView", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "DayWiseFeeCollectionPrincipalView", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindDayWiseFeeCollectionPrincipalView(string sDateFrom, string sDateTo, string sFrequencyId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            string sSql = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
                    objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchDayWiseFeeCollection).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, sFrequencyId);
                    var FetchClassWiseFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, sSql, sAcademicYear).DataSource.Data;
                    if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                        objViewModel.liFeeTransaction = FetchClassWiseFeeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "BindDayWiseFeeCollectionPrincipalView", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "BindDayWiseFeeCollectionPrincipalView", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult ViewPaymentDetailsByDate(string sPaymentDate, string sFrequency)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objFeeStudentAccountModel.PAYMENT_DATE = CommonMethods.ConvertdatetoyearFromat(sPaymentDate);
                    objFeeStudentAccountModel.FREQUENCY = sFrequency;
                    var FetchClassWiseFeeDetails = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeePaymentDetailsByDate), sAcademicYear).DataSource.Data;
                    if (FetchClassWiseFeeDetails != null && FetchClassWiseFeeDetails.Count > 0)
                        objViewModel.liFeeTransaction = FetchClassWiseFeeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ViewPaymentDetailsByDate", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ViewPaymentDetailsByDate", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Fee Refunded List
        [UserRights("PRINCIPAL")]
        public ActionResult FeeRefundedList()
        {
            FeeViewModel ObjViewModel = new FeeViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    ObjViewModel.liFeeRefunded = (List<FEE_COLLECTION_DELETED>)CMSHandler.FetchData<FEE_COLLECTION_DELETED>(null, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeRefundedList), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionCanceledList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionCanceledList", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(ObjViewModel);
        }
        #endregion
        #region Fee Dues
        public ActionResult FeeBalanceList()
        {
            FeeTransactionViewModel liFeeTransactions = new FeeTransactionViewModel();
            var listatusverification = new List<FEE_STUDENT_ACCOUNT>();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {

                    liFeeTransactions.liFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() },
                    FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeTransaction), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    if (liFeeTransactions.liFeeStudentAccount != null && liFeeTransactions.liFeeStudentAccount.Count > 0)
                    {
                        foreach (var item in liFeeTransactions.liFeeStudentAccount)
                        {
                            if (item.FREQUENCY_TYPE_ID == Common.FrequencyType.AdmissionFee)
                            {
                                var admissionstatus = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = item.STUDENT_ID, FREQUENCY_ID = item.FREQUENCY_ID, STATUS = Common.STATUS.Verified }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAdmissionStatusByStudentIdandFrequencyId)).DataSource.Data;
                                if (admissionstatus != null && admissionstatus.Count > 0)
                                {
                                    listatusverification.Add(new FEE_STUDENT_ACCOUNT() { FREQUENCY_TYPE_ID = item.FREQUENCY_TYPE_ID, FREQUENCY_ID = item.FREQUENCY_ID, STATUS = admissionstatus.FirstOrDefault().STATUS, STATUS_NAME = admissionstatus.FirstOrDefault().STATUS_NAME });
                                }

                            }
                            if (item.FREQUENCY_TYPE_ID == Common.FrequencyType.AdmissionFeeSSC)
                            {
                                var admissionstatus = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = item.STUDENT_ID, FREQUENCY_ID = item.FREQUENCY_ID, STATUS = Common.STATUS.Verified }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAdmissionStatusByStudentIdandFrequencyId)).DataSource.Data;
                                if (admissionstatus != null && admissionstatus.Count > 0)
                                {
                                    listatusverification.Add(new FEE_STUDENT_ACCOUNT() { FREQUENCY_TYPE_ID = item.FREQUENCY_TYPE_ID, FREQUENCY_ID = item.FREQUENCY_ID, STATUS = admissionstatus.FirstOrDefault().STATUS, STATUS_NAME = admissionstatus.FirstOrDefault().STATUS_NAME });
                                }

                            }
                            else if (item.FREQUENCY_TYPE_ID == Common.FrequencyType.HostelFee)
                            {
                                var hostelstatus = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = item.STUDENT_ID, FREQUENCY = item.FREQUENCY_ID, }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchHostelAdmissionStatusByStudentId), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                if (hostelstatus != null && hostelstatus.Count > 0)
                                {
                                    listatusverification.Add(new FEE_STUDENT_ACCOUNT() { FREQUENCY_TYPE_ID = item.FREQUENCY_TYPE_ID, STATUS = hostelstatus.FirstOrDefault().STATUS, STATUS_NAME = hostelstatus.FirstOrDefault().STATUS_NAME });
                                }
                            }
                        }
                    }

                    liFeeTransactions.liStatusVerification = listatusverification;
                }
                catch (Exception e)
                {
                    string sErrorMessage = e.Message;
                }

            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

            return View(liFeeTransactions);
        }
        #endregion
        #region Fee Reciept
        public ActionResult FeeReceipt(string sFrequencyId, string sFrequencytypeId)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            var objFeeStudentAccountModel = new FEE_STUDENT_ACCOUNT();
            var objFeeReceipt = new FEE_RECEIPT_GENERATION();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {

                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                try
                {
                    objFeeReceipt.STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objFeeReceipt.FREQUENCY_ID = sFrequencyId;
                    objViewModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    if (sFrequencytypeId == Common.FrequencyType.AdmissionApplicationFee)
                    {
                        objViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeReceipt, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicationTransactionByReceiveId), sAcademicYear).DataSource.Data;
                    }
                    else
                        objViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeReceipt, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByIssueId), sAcademicYear).DataSource.Data;

                    int COUNT = 0;
                    foreach (var item in objViewModel.liFeeReceiptGeneration)
                    {

                        objFeeStudentAccountModel.TRANSACTION_ID = item.TRANSACTION_ID;
                        objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                        objFeeStudentAccountModel.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        var LstfeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeReceiptByTransactionId), sAcademicYear).DataSource.Data;
                        if (LstfeeTransaction != null && LstfeeTransaction.Count > 0)
                        {

                            if (COUNT == 0)
                            {
                                objViewModel.liFeeTransaction = LstfeeTransaction;
                            }
                            else
                            {
                                objViewModel.liFeeTransaction = objViewModel.liFeeTransaction.Concat(LstfeeTransaction).ToList();
                            }
                            COUNT++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "HostelFeeReceipt", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "HostelFeeReceipt", ex.Message);
                    }

                }
            }
            return View(objViewModel);
        }
        #endregion
        #region FeeStructureReportByProgramme
        public ActionResult FeeStructureReportByProgramme()
        {
            FeeStructureViewModel objViewModel = new FeeStructureViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var SupShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift), sAcademicYear).DataSource.Data;
                    var SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    var FetchProgrammeGroup = (List<SUP_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<SUP_APPTYPE_PROG_GROUPS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupProgramme)).DataSource.Data;
                    var SupFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeMode), sAcademicYear).DataSource.Data;
                    var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                    if (FetchProgrammeGroup != null && FetchProgrammeGroup.Count > 0)
                        objViewModel.ProgrammeGroupList = new SelectList(FetchProgrammeGroup, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_NAME);
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.AcademicyearList = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                    if (FetchClassYear != null && FetchClassYear.Count > 0)
                        objViewModel.ClassYearList = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                    if (SupShift != null && SupShift.Count > 0)
                        objViewModel.ShiftList = new SelectList(SupShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    if (SupProgrammeMode != null && SupProgrammeMode.Count > 0)
                        objViewModel.ProgrammeModeList = new SelectList(SupProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                    if (SupFrequency != null && SupFrequency.Count > 0)
                        objViewModel.FrequencyList = new SelectList(SupFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeeStructureReportByProgramme", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeeStructureReportByProgramme", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult BindProgrammeGroupByShiftAndProgrammeMode(string sShift, string sProgrammeMode)
        {
            string sOption = string.Empty;
            CP_PROGRAMME_GROUP objModel = new CP_PROGRAMME_GROUP();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.SHIFT = sShift;
                    objModel.PROGRAMME_MODE = sProgrammeMode;
                    var Programme = (List<ADM_APPTYPE_PROG_GROUPS>)CMS.DAO.CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupByShiftAndProgrammeMode)).DataSource.Data;
                    if (Programme != null && Programme.Count > 0)
                    {
                        foreach (var item in Programme)
                        {
                            sOption += "<option value='" + item.PROGRAMME_GROUP_ID + "' >" + item.PROGRAMME_NAME + "</option>";
                        }
                    }
                }
                else
                    return Json(sOption);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", ex.Message);
                }
            }
            return Json(sOption);
        }
        public ActionResult BindFeeStructureReportByProgramme(string sAcademicYearId, string sFrequecyId, string sClassYear, string sShift, string sProgrammeMode, string sProgrammeId)
        {
            FeeStructureViewModel ObjViewModel = new FeeStructureViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            string sSql = string.Empty;
            string Sql = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                FEE_STRUCTURE objModel = new FEE_STRUCTURE();
                try
                {
                    objModel.ACADEMIC_YEAR = sAcademicYearId;
                    objModel.FREQUENCY_ID = sFrequecyId;
                    objModel.SHIFT = sShift;
                    objModel.PROGRAMME_MODE = sProgrammeMode;
                    sSql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStructureReportByProgrammeWiseReport).Replace(Common.Delimiter.QUS + "CLASS_YEAR_ID", sClassYear);
                    sSql = sSql.Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeId);
                    Sql = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeadForFeeStructureByGroup).Replace(Common.Delimiter.QUS + "CLASS_YEAR_ID", sClassYear);
                    Sql = Sql.Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeId);
                    ObjViewModel.lstFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, sSql, sAcademicYear).DataSource.Data;
                    ObjViewModel.LiFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, Sql, sAcademicYear).DataSource.Data;
                }
                catch (Exception Ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "BindFeeStructureReportByProgramme", "Err MSg: " + Ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "BindFeeStructureReportByProgramme", Ex.Message);
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");

            return View(ObjViewModel);
        }
        #endregion

        #region DateWiseFeeReport
        public ActionResult MainHeadwiseReportByDate()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                if (FetchClassYear != null && FetchClassYear.Count > 0)
                    ObjViewModel.liClassYear = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                    ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                var FectchFeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                if (FectchFeeCategory != null && FectchFeeCategory.Count > 0)
                    ObjViewModel.FeeCategory = new SelectList(FectchFeeCategory, Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, Common.SUP_FEE_CATEGORY.FEE_CATEGORY);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindHeadwiseReportByDate(string sAcYear, string sProgrammeGroupId, string sFrequency, string sFrequencyType, string sDateFrom, string sDateTo, string sProgrameMode, string sShift)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var ObjModel = new FEE_REPORT()
                {
                    FREQUENCY_TYPE = sFrequencyType,
                    FREQUENCY = sFrequency,
                    ACADEMIC_YEAR = sAcYear,
                    FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                    DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                    DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                    SHIFT = sShift,
                    PROGRAMME_MODE = sProgrameMode
                };

                // Fetch Student Details ..
                string sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentsForNewDateReport).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Main Heads ..
                 sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadsByPRogrammeGroupIdAndFrequencyId).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.lstSupMainHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch SubHeads By ProgrammeGroupId ..
                sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadsByFeeRootIdAndProgrammeGroupId).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.lstSupHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Headswise Fee Report ..                                          
                ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdForNewDateWiseReport), sAcademicYear).DataSource.Data;
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        #endregion

        #region FeecategoryWiseReport
        public ActionResult MainHeadwiseReportByFeecategoryForCollege()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                if (FetchClassYear != null && FetchClassYear.Count > 0)
                    ObjViewModel.liClassYear = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                    ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                var FectchFeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                if (FectchFeeCategory != null && FectchFeeCategory.Count > 0)
                    ObjViewModel.FeeCategory = new SelectList(FectchFeeCategory, Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, Common.SUP_FEE_CATEGORY.FEE_CATEGORY);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindHeadwiseFeeReportByFeecategory(string sAcYear, string sProgrammeGroupId, string sFrequency, string sFrequencyType, string sClassYear, string sDateFrom, string sDateTo, string sFeeCategoryId, string sProgrameMode, string sShift)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var ObjModel = new FEE_REPORT()
                {
                    FREQUENCY_TYPE = sFrequencyType,
                    FREQUENCY = sFrequency,
                    ACADEMIC_YEAR = sAcYear,
                    FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                    DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                    DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                    SHIFT = sShift,
                    PROGRAMME_MODE = sProgrameMode,
                    FEE_CATEGORY_ID = sFeeCategoryId
                };

                // Fetch Student Details ..
                string sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentByProgrammeGroupIdForDateReport).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, ObjModel.FEE_CATEGORY_ID);
                ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Main Heads ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadsByPRogrammeGroupIdAndFrequencyIdAndFeeCategory).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, ObjModel.FEE_CATEGORY_ID);
                ObjViewModel.lstSupMainHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch SubHeads By ProgrammeGroupId ..
                sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadsByFeeRootIdAndProgrammeGroupIdAndFeeCategory).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, ObjModel.FEE_CATEGORY_ID);
                ObjViewModel.lstSupHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Headswise Fee Report ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdAndFeeCategory).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, ObjModel.FEE_CATEGORY_ID);
                ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        #endregion

        #region Termwise Report
        public ActionResult MainHeadwiseReportForCollege()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                if (FetchClassYear != null && FetchClassYear.Count > 0)
                    ObjViewModel.liClassYear = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);
                var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                    ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindHeadwiseFeeReport(string sAcYear, string sProgrammeGroupId, string sFrequencyMode, string sFrequencyType, string sClassYear, string sDateFrom, string sDateTo, string sShift, string sProgrammeMode)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var ObjModel = new FEE_REPORT()
                {
                    SHIFT = sShift,
                    PROGRAMME_MODE = sProgrammeMode,
                    FREQUENCY = sFrequencyMode,
                    FREQUENCY_TYPE = sFrequencyType,
                    ACADEMIC_YEAR = sAcYear,
                    FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                    CLASS_YEAR = sClassYear,
                    DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                    DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo)
                };

                // Fetch Student Details ..
                string sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentByProgrammeGroupIdForTermFeeReport).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Main Heads ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadsByPRogrammeGroupIdAndFrequencyId).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.lstSupMainHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch SubHeads By ProgrammeGroupId ..
                sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadsByFeeRootIdAndProgrammeGroupId).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeGroupId);
                ObjViewModel.lstSupHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Headswise Fee Report ..                                          
                ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdForDateWiseReport), sAcademicYear).DataSource.Data;
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        #endregion

        #region FeeAccountDateWiseReport
        public ActionResult AccountWiseSettlement()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            SUP_BANK_ACCOUNT obj = new SUP_BANK_ACCOUNT();
            string userId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    if (userId == "45")
                    {
                        obj.BANK_ACCOUNT_ID = "11,12,13,14,15,16,17,18,19";
                        var sql = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccountsProgrammewise).Replace(Common.Delimiter.QUS + "BANK_ACCOUNT_ID", obj.BANK_ACCOUNT_ID);

                        var LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, sql).DataSource.Data;
                        objViewModel.liBankAccount = new SelectList(LiSupBankAccount, Common.SUP_BANK_ACCOUNT.BANK_ACCOUNT_ID, Common.SUP_BANK_ACCOUNT.PASSBOOK_NO);
                    }
                    else if (userId == "47")
                    {
                        obj.BANK_ACCOUNT_ID = "20";
                        var sql = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccountsProgrammewise).Replace(Common.Delimiter.QUS + "BANK_ACCOUNT_ID", obj.BANK_ACCOUNT_ID);
                        var LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, sql).DataSource.Data;
                        objViewModel.liBankAccount = new SelectList(LiSupBankAccount, Common.SUP_BANK_ACCOUNT.BANK_ACCOUNT_ID, Common.SUP_BANK_ACCOUNT.PASSBOOK_NO);
                    }
                    else
                    {
                        var LiSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccounts)).DataSource.Data;
                        objViewModel.liBankAccount = new SelectList(LiSupBankAccount, Common.SUP_BANK_ACCOUNT.BANK_ACCOUNT_ID, Common.SUP_BANK_ACCOUNT.PASSBOOK_NO);
                    }

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AccountWiseSettlement", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AccountWiseSettlement", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAccountWiseSettlement(string sFrom, string sTo, string sBankAccount)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            FEE_REPORT objModel = new FEE_REPORT();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sFrom);
                    objModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sTo);
                    objModel.BANK_ACCOUNT_ID = sBankAccount;
                    objViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(objModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSettlementByDateWise), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindAccountWiseSettlement", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindAccountWiseSettlement", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAccountWiseSettlementByDate(string sSettlementID, string sBankAccount)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            FEE_REPORT objModel = new FEE_REPORT();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(new FEE_REPORT() { RECIPIENT_SETTLEMENT_ID = sSettlementID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchSettlementByDateWiseAndAccount), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindAccountWiseSettlementByDate", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindAccountWiseSettlementByDate", "Fee", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region APPLICATION FEE Report
        public ActionResult ApplicationFeeByDate()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string userId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FetchClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear), sAcademicYear).DataSource.Data;
                if (FetchClassYear != null && FetchClassYear.Count > 0)
                    ObjViewModel.liClassYear = new SelectList(FetchClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.CP_CLASSES_2017.CLASS_YEAR);

                if (userId == "45")
                {
                    var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeModeRegular)).DataSource.Data;
                    if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                        ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                else if (userId == "47")
                {
                    var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeModeSSC)).DataSource.Data;
                    if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                        ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                else
                {
                    var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                    if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                        ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }


                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                var FectchFeeCategory = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFeeCategory)).DataSource.Data;
                if (FectchFeeCategory != null && FectchFeeCategory.Count > 0)
                    ObjViewModel.FeeCategory = new SelectList(FectchFeeCategory, Common.SUP_FEE_CATEGORY.FEE_CATEGORY_ID, Common.SUP_FEE_CATEGORY.FEE_CATEGORY);
                var FetchApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                if (FetchApplicationType != null && FetchApplicationType.Count > 0)
                    ObjViewModel.liApplicationType = new SelectList(FetchApplicationType, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindApplicationFeeReportByDate(string sAcYear, string sApplicationType, string sFrequency, string sFrequencyType, string sDateFrom, string sDateTo, string sProgrameMode, string sShift)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            //string sAcademicYear = sAcYear;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var ObjModel = new FEE_REPORT()
                {
                    FREQUENCY_TYPE = sFrequencyType,
                    FREQUENCY = sFrequency,
                    ACADEMIC_YEAR = sAcYear,
                    FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                    DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                    DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                    SHIFT = sShift,
                    PROGRAMME_MODE = sProgrameMode,
                    APPTYPE_ID = sApplicationType
                };
                string sSQL = string.Empty;
                if (ObjModel.FREQUENCY == "2")
                {
                    // Fetch Student Details ..
                    //string sSQL = string.Empty;
                    sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAppliedStudentByProgrammeModeAndShift);
                    ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                {
                    sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAdmittedStudentByProgrammeModeAndShift);
                    ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;
                }


                // Fetch Main Heads ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadsForApplicationFeebByFrequencyId);
                ObjViewModel.lstSupMainHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch SubHeads By ProgrammeGroupId ..
                sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadsForApplicationFee);
                ObjViewModel.lstSupHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Headswise Fee Report ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchHeadWiseForApplicationFee);
                ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, sSQL, sAcademicYear).DataSource.Data;
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        public ActionResult ApplicationFeeCountByDate()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FectchApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                if (FectchApplicationType != null && FectchApplicationType.Count > 0)
                    ObjViewModel.liApplicationType = new SelectList(FectchApplicationType, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE);
                var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                    ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindApplicationFeeCountByDate(string sAcYear, string sFrequency, string sDateFrom, string sDateTo, string sProgrameMode, string sShift, string sApplicationType)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ObjModel = new FEE_REPORT()
                    {
                        FREQUENCY = sFrequency,
                        ACADEMIC_YEAR = sAcYear,
                        FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                        DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                        DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                        SHIFT = sShift,
                        PROGRAMME_MODE = sProgrameMode,
                        APPTYPE_ID = sApplicationType
                    };
                    ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicationFeeCountByDate), sAcademicYear).DataSource.Data;
                }
                catch (Exception Ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "BindApplicationFeeCountByDate", "Err MSg: " + Ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "BindApplicationFeeCountByDate", Ex.Message);
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        public ActionResult ApplicationFeeCountByProgrammeMode()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                var FectchApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                if (FectchApplicationType != null && FectchApplicationType.Count > 0)
                    ObjViewModel.liApplicationType = new SelectList(FectchApplicationType, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE);
                var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                    ObjViewModel.liProgrammeModeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                if (FetchShift != null && FetchShift.Count > 0)
                    ObjViewModel.liShift = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindApplicationFeeCountByProgrammeMode(string sAcYear, string sFrequency, string sDateFrom, string sDateTo, string sProgrameMode, string sShift, string sApplicationType)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ObjModel = new FEE_REPORT()
                    {
                        FREQUENCY = sFrequency,
                        ACADEMIC_YEAR = sAcYear,
                        FEE_ROOT_ID = Common.FeeRoot.CollegeFee,
                        DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                        DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                        SHIFT = sShift,
                        PROGRAMME_MODE = sProgrameMode,
                        APPTYPE_ID = sApplicationType
                    };
                    ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicationFeeCountByProgrammeMode), sAcademicYear).DataSource.Data;
                }
                catch (Exception Ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Fee", "BindApplicationFeeCountByProgrammeMode", "Err MSg: " + Ex.Message, "Query is empty!");
                        objHandler.WriteError("Fee", "BindApplicationFeeCountByProgrammeMode", Ex.Message);
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        #endregion

        #region Hostel DateWiseReport
        public ActionResult MainHeadwiseReportByDateForHostel()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                    ObjViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);

                //var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllHostelFrequency), sAcademicYear).DataSource.Data;
                //if (FetchFrequency != null && FetchFrequency.Count > 0)
                //    ObjViewModel.liSupFeeFrequency = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);

                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public ActionResult BindHeadwiseReportByDateForHostel(string sAcYear, string sFrequency, string sFrequencyType, string sDateFrom, string sDateTo)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjViewModel = new FeeTransactionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                var ObjModel = new FEE_REPORT()
                {
                    FREQUENCY_TYPE = sFrequencyType,
                    FREQUENCY = sFrequency,
                    ACADEMIC_YEAR = sAcYear,
                    FEE_ROOT_ID = Common.FeeRoot.HostelFee,
                    DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom),
                    DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo),
                };

                // Fetch Student Details ..
                string sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentByProgrammeGroupIdForDateReport);
                ObjViewModel.LiReceivedStudentInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Main Heads ..
                sSQL = string.Empty;
                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadsForHostelFee);
                ObjViewModel.lstSupMainHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch SubHeads By ProgrammeGroupId ..
                sSQL = string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHeadsForHostelFee);
                ObjViewModel.lstSupHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjModel, sSQL, sAcademicYear).DataSource.Data;

                // Fetch Headswise Fee Report ..                                          
                ObjViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(ObjModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchMainHeadwiseFeeReportForHostelFee), sAcademicYear).DataSource.Data;
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }
        #endregion

        // DATE WISE FEE RECEIPT - FOR SSC-SMC 

        #region DateWiseReceipt
        public ActionResult ListDateWiseStudentFeeReceipt()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();

            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    var SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liFrequencyType = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (SupProgrammeMode != null && SupProgrammeMode.Count > 0)
                        objViewModel.liProgrammeModeList = new SelectList(SupProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListStudentAccount", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListStudentAccount", Ex.Message);
                }

            }
            return View(objViewModel);
        }

        public ActionResult BindFeeReceiptByDate(string sFrequencyId, string sAcYear, string sDateFrom, string sDateTo, string sProgrammeMode)
        {
            FeeTransactionViewModel ObjViewModel = new FeeTransactionViewModel();
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                    objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
                    objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
                    objFeeStudentAccountModel.PROGRAMME_MODE = sProgrammeMode;
                    ObjViewModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;

                    var liFrequencyType = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FREQUENCY_ID = objFeeStudentAccountModel.FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyType), sAcademicYear).DataSource.Data;


                    if (liFrequencyType != null && liFrequencyType.Count > 0)
                    {
                        if (liFrequencyType.FirstOrDefault().FEE_MODE == Common.FrequencyType.AdmissionApplicationFee)
                        {
                            ObjViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicationTransactionByDate), sAcademicYear).DataSource.Data;

                        }
                        else
                        {
                            ObjViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByDate), sAcademicYear).DataSource.Data;

                        }
                    }



                    // objViewModel.liFeeReport = (List<FEE_REPORT>)CMSHandler.FetchData<FEE_REPORT>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentTransactionByDate), sAcademicYear).DataSource.Data;
                    if (ObjViewModel.liFeeReceiptGeneration != null && ObjViewModel.liFeeReceiptGeneration.Count > 0)
                    {
                        int COUNT = 0;
                        foreach (var item in ObjViewModel.liFeeReceiptGeneration)
                        {

                            objFeeStudentAccountModel.TRANSACTION_ID = item.TRANSACTION_ID;
                            objFeeStudentAccountModel.FREQUENCY_ID = item.FREQUENCY_ID;
                            objFeeStudentAccountModel.ACADEMIC_YEAR = sAcYear;
                            var LstfeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeReceiptByTransactionId), sAcademicYear).DataSource.Data;
                            if (LstfeeTransaction != null && LstfeeTransaction.Count > 0)
                            {
                                if (COUNT == 0)
                                {
                                    ObjViewModel.liFeeTransaction = LstfeeTransaction;
                                }
                                else
                                {
                                    ObjViewModel.liFeeTransaction = ObjViewModel.liFeeTransaction.Concat(LstfeeTransaction).ToList();
                                }
                                COUNT++;
                            }
                        }
                    }

                    sAcademicYear = sAcYear;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("BindFeeReceiptByDate", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("BindFeeReceiptByDate", "Fee", ex.Message);
                    }
                }
            }
            return View(ObjViewModel);
        }


        // FOR REGULAR HEAD WISE

        public ActionResult ListHeadWiseStudentFeeReceipt()
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchAcademicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
                    var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(new SUP_FEE_FREQUENCY() { FEE_ROOT_ID = Common.FeeRoot.CollegeFee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId), sAcademicYear).DataSource.Data;
                    var SupProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    if (FetchAcademicYear != null && FetchAcademicYear.Count > 0)
                        objViewModel.liAcademicYear = new SelectList(FetchAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objViewModel.liFrequencyType = new SelectList(FetchFrequency, Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, Common.SUP_FEE_FREQUENCY.FREQUENCY_NAME);
                    if (SupProgrammeMode != null && SupProgrammeMode.Count > 0)
                        objViewModel.liProgrammeModeList = new SelectList(SupProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "ListHeadWiseStudentFeeReceipt", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "ListHeadWiseStudentFeeReceipt", Ex.Message);
                }

            }
            return View(objViewModel);
        }

        public JsonResult BindMainHeadByFrequencyID(string sAcYear, string sFrequency, string sProgammeMode)
        {
            FeeTransactionViewModel objViewModel = new FeeTransactionViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = sAcYear;
                    var FeeHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(new SUP_FEE_MAIN_HEAD() { FREQUENCY_ID = sFrequency, PROGRAMME_MODE = sProgammeMode }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainHeadList), sAcademicYear).DataSource.Data;

                    if (FeeHead != null && FeeHead.Count > 0)
                    {
                        foreach (var item in FeeHead)
                            objResult.sResult += "<option value='" + item.MAIN_HEAD_ID + "'>" + item.MAIN_HEAD + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Fee", "BindMainHeadByFrequencyID", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Fee", "BindMainHeadByFrequencyID", Ex.Message);
                }

            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BindFeeReceiptByHeadandDateWise(string sFrequencyId, string sAcYear, string sDateFrom, string sDateTo, string sProgrammeMode, string sHead)
        {
            FeeTransactionViewModel ObjViewModel = new FeeTransactionViewModel();
            var objFeeStudentAccountModel = new FEE_TRANSACTION();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    objFeeStudentAccountModel.FREQUENCY_ID = sFrequencyId;
                    objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
                    objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
                    objFeeStudentAccountModel.PROGRAMME_MODE = sProgrammeMode;
                    ObjViewModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;

                    var liFrequencyType = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FREQUENCY_ID = objFeeStudentAccountModel.FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyType), sAcademicYear).DataSource.Data;


                    if (liFrequencyType != null && liFrequencyType.Count > 0)
                    {
                        if (liFrequencyType.FirstOrDefault().FEE_MODE == Common.FrequencyType.AdmissionApplicationFee)
                        {
                            ObjViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchApplicationTransactionByDate), sAcademicYear).DataSource.Data;

                        }
                        else
                        {
                            ObjViewModel.liFeeReceiptGeneration = (List<FEE_RECEIPT_GENERATION>)CMSHandler.FetchData<FEE_RECEIPT_GENERATION>(objFeeStudentAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTransactionByDate), sAcademicYear).DataSource.Data;

                        }
                    }
                    if (ObjViewModel.liFeeReceiptGeneration != null && ObjViewModel.liFeeReceiptGeneration.Count > 0)
                    {
                        int COUNT = 0;
                        foreach (var item in ObjViewModel.liFeeReceiptGeneration)
                        {

                            objFeeStudentAccountModel.TRANSACTION_ID = item.TRANSACTION_ID;
                            objFeeStudentAccountModel.FREQUENCY_ID = item.FREQUENCY_ID;
                            objFeeStudentAccountModel.ACADEMIC_YEAR = sAcYear;

                            string sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeReceiptByTransactionIdAndHead).Replace(Common.Delimiter.QUS + Common.FEE_MAIN_HEADS.MAIN_HEAD_ID, sHead);
                            var LstfeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(objFeeStudentAccountModel, sSQL, sAcademicYear).DataSource.Data;
                            if (LstfeeTransaction != null && LstfeeTransaction.Count > 0)
                            {
                                if (COUNT == 0)
                                {
                                    ObjViewModel.liFeeTransaction = LstfeeTransaction;
                                }
                                else
                                {
                                    ObjViewModel.liFeeTransaction = ObjViewModel.liFeeTransaction.Concat(LstfeeTransaction).ToList();
                                }
                                COUNT++;
                            }
                        }
                    }

                    sAcademicYear = sAcYear;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("BindFeeReceiptByDate", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("BindFeeReceiptByDate", "Fee", ex.Message);
                    }
                }
            }
            return View(ObjViewModel);
        }
        #endregion        
        #region RazorpayPaymentUpdate
        public string RazorpayPaymentUpdateByids()
        {
            string[] payId = @"pay_O7COSgAYM8KHLA, pay_O7ZSptnGUdIzBZ, pay_O9w5ZoLFs8pdc6, pay_OAjJ9MqKWjwOeu, pay_OB4931nCMnDiv5, pay_OBCjUfdvrGAjv5, pay_ODS3XimgRNecLV, pay_ODqGT4ELYOKGcd, pay_OEDSp9ADpLh28z, pay_OEDWdlCOy7KGs0, pay_OEDY7pa1a6dhzU".Split(',');

            for (int i = 0; i < payId.Length; i++)
            {
                RazorPayPaymentResponse(payId[i].Trim());
            }
            return "Done";
        }
        public string RazorpayTransferUpdateByids()
        {
            string[] payId = @"1314".Split(',');

            for (int i = 0; i < payId.Length; i++)
            {
                FeeRazorpayTransferAccount(payId[i].Trim(), "1", "2024");
            } 
            return "Done";
        }
        #endregion

    }
}