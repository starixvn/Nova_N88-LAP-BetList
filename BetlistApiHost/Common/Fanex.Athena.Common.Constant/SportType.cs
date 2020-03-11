using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Common.Constant
{
    public enum SportType
    {
        /// <summary>
        /// None, value = 0.
        /// </summary>
        None = 0,

        /// <summary>
        /// Soccer sport, value = 1.
        /// </summary>
        Soccer = 1,

        /// <summary>
        /// BasketBall sport, value = 2.
        /// </summary>
        BasketBall = 2,

        /// <summary>
        /// FootBall sport, value = 3.
        /// </summary>
        FootBall = 3,

        /// <summary>
        /// IceHockey sport, value = 4.
        /// </summary>
        IceHockey = 4,

        /// <summary>
        /// Tennis sport, value = 5.
        /// </summary>
        Tennis = 5,

        /// <summary>
        /// VolleyBall sport, value = 6.
        /// </summary>
        VolleyBall = 6,

        /// <summary>
        /// Billiard sport, value = 7.
        /// </summary>
        Billiard = 7,

        /// <summary>
        /// BasketBall sport, value = 8.
        /// </summary>
        BaseBall = 8,

        /// <summary>
        /// Badminton sport, value = 9.
        /// </summary>
        Badminton = 9,

        /// <summary>
        /// Golf sport, value = 10.
        /// </summary>
        Golf = 10,

        /// <summary>
        /// BasketBall sport, value = 11.
        /// </summary>
        Motorsports = 11,

        /// <summary>
        /// Swimming sport, value = 12.
        /// </summary>
        Swimming = 12,

        /// <summary>
        /// Politics sport, value = 13.
        /// </summary>
        Politics = 13,

        /// <summary>
        /// WaterPolo sport, value = 14.
        /// </summary>
        WaterPolo = 14,

        /// <summary>
        /// Diving sport, value = 15.
        /// </summary>
        Diving = 15,

        /// <summary>
        /// Boxing sport, value = 16.
        /// </summary>
        Boxing = 16,

        /// <summary>
        /// Archery sport, value = 17.
        /// </summary>
        Archery = 17,

        /// <summary>
        /// TableTennis sport, value = 18.
        /// </summary>
        TableTennis = 18,

        /// <summary>
        /// Weightlifting sport, value = 19.
        /// </summary>
        Weightlifting = 19,

        /// <summary>
        /// Canoeing sport, value = 20.
        /// </summary>
        Canoeing = 20,

        /// <summary>
        /// Gymnastics sport, value = 21.
        /// </summary>
        Gymnastics = 21,

        /// <summary>
        /// BasketBall sport, value = 22.
        /// </summary>
        Athletics = 22,

        /// <summary>
        /// Equestrian sport, value = 23.
        /// </summary>
        Equestrian = 23,

        /// <summary>
        /// Handball sport, value = 24.
        /// </summary>
        Handball = 24,

        /// <summary>
        /// Rugby sport, value = 26.
        /// </summary>
        Rugby = 26,

        /// <summary>
        /// Cricket sport, value = 27.
        /// </summary>
        Cricket = 27,

        /// <summary>
        /// NetBall sport, value = 32.
        /// </summary>
        NetBall = 32,

        /// <summary>
        /// FieldHockey sport, value = 28.
        /// </summary>
        FieldHockey = 28,

        /// <summary>
        /// OtherSports sport, value = 99.
        /// </summary>
        OtherSports = 99,

        /// <summary>
        /// Finance sport, value = 2.
        /// </summary>
        Finance = 201,

        /// <summary>
        /// Casino sport, value = 251.
        /// </summary>
        Casino = 251,

        /// <summary>
        /// Squash sport, value = 30.
        /// </summary>
        Squash = 30,

        /// <summary>
        /// Winter sport, value = 29.
        /// </summary>
        Winter = 29,

        /// <summary>
        /// Entertainment sport, value = 31.
        /// </summary>
        Entertainment = 31,

        /// <summary>
        /// Cycling sport, value = 33.
        /// </summary>
        Cycling = 33,

        /// <summary>
        /// Fencing sport, value = 34.
        /// </summary>
        Fencing = 34,

        /// <summary>
        /// Judo sport, value = 35.
        /// </summary>
        Judo = 35,

        /// <summary>
        /// ModernPentathlon sport, value = 36.
        /// </summary>
        ModernPentathlon = 36,

        /// <summary>
        /// Rowing sport, value = 37.
        /// </summary>
        Rowing = 37,

        /// <summary>
        /// Sailing sport, value = 38.
        /// </summary>
        Sailing = 38,

        /// <summary>
        /// Shooting sport, value = 39.
        /// </summary>
        Shooting = 39,

        /// <summary>
        /// Taekwondo sport, value = 40.
        /// </summary>
        Taekwondo = 40,

        /// <summary>
        /// Triathlon sport, value = 41.
        /// </summary>
        Triathlon = 41,

        /// <summary>
        /// Wrestling sport, value = 42.
        /// </summary>
        Wrestling = 42,

        Esports = 43,

        UnknowThaiSport = 46,

        /// <summary>
        /// NumberGame sport, value = 161.
        /// </summary>
        NumberGame = 161,

        /// <summary>
        /// LiveCasino sport, value = 162.
        /// </summary>
        LiveCasino = 162,

        /// <summary>
        /// Harness sport, value = 153.
        /// </summary>
        Harness = 153,

        HorseRacingFixedOdds = 154,

        /// <summary>
        /// Greyhound sport, value = 152.
        /// </summary>
        Greyhound = 152,

        /// <summary>
        /// Number Game
        /// </summary>
        NumberGameHappy5 = 164,

        /// <summary>
        /// VirtualSport, value = 180.
        /// </summary>
        VirtualSport = 180,

        /// <summary>
        /// VirtualHorseRacing sport, value = 181.
        /// </summary>
        VirtualHorseRacing = 181,

        /// <summary>
        /// VirtualGreyhound sport, value = 181.
        /// </summary>
        VirtualGreyhound = 182,

        /// <summary>
        /// Value = 183.
        /// </summary>
        VirtualSpeedway = 183,

        /// <summary>
        /// VirtualMotoRacing sport, value = 181.
        /// </summary>
        VirtualMotoRacing = 184,

        /// <summary>
        /// Value = 185.
        /// </summary>
        VirtualCycling = 185,

        /// <summary>
        /// Keno product ,value=202.
        /// </summary>
        Keno = 202,

        AllBet = 211,

        Voidbridge = 212,

        FishingWorld = 219,

        Lottery = 220,

        Exchange = 221,

        VirtualBasketball = 193,

        PP = 248
    }
}
