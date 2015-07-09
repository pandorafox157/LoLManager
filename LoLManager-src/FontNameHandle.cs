using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLManager
{
    enum FontSettingHandle
    {
        Decay = 0,
        ContinualForceY = 1,
        MinYVelocity = 2,
        MaxYVelocity = 3,
        ShrinkScale = 4,
        ShrinkTime = 5,
    }
    enum FontNameHandle
    {
        UnspecifiedFont = 0,
        Heal = 1,
        ManaHeal = 2,
        ManaDamage = 3,
        Dodge = 4,
        Gold = 5,
        Special = 6,
        Experience = 7,
        Level = 8,
        Invulnerable = 9,
        Critical = 10,
        PhysicalDamage = 11,
        MagicalDamage = 12,
        TrueDamage = 13,
        EnemyCritical = 14,
        EnemyPhysicalDamage = 15,
        EnemyMagicalDamage = 16,
        EnemyTrueDamage = 17,
        Countdown = 18,
        LegacyDamage = 19,
        LegacyCritical = 20,
        AnnouncementEpic = 21,
        AnnouncementLargeZoomIn = 22,
        AnnouncementPentakill = 23,
        AnnouncementMajor = 24,
        AnnouncementMinor = 25,
        AnnouncementTencentWarning = 26,
        OMW = 27
    }
    enum GeneralCharacterDataTagNameHandle
    {
        HealthBarChampionSelfDefault = 0,
        HealthBarChampionFriendlyDefault = 1,
        HealthBarChampionEnemyDefault = 2,
        HealthBarChampionSelfColorblind = 3,
        HealthBarChampionFriendlyColorblind = 4,
        HealthBarChampionEnemyColorblind = 5,
    }
    enum ChatTypeNameHandle
    {
        PlayerNametag = 0,
        FriendlyNametag = 1,
        EnemyNametag = 2,
    }

    enum FontTypeHandle
    {
        FZLTCH = 0,
        FZXHYSZK = 1,
    }
}
