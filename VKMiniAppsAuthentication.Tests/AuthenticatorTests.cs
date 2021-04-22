using NUnit.Framework;
// ReSharper disable StringLiteralTypo

namespace VKMiniAppsAuthentication.Tests
{
    public sealed class Tests
    {
        private const string Secret = "5rwT5fZEgg12XT5e2pAj";

        [Test]
        public void BasicCorrectTest()
        {
            const string parameters =
                "vk_access_token_settings=photos,menu&vk_app_id=7605622&vk_are_notifications_enabled=1&vk_is_app_user=1&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147&sign=BEZLyBbmG-CjEsunjTPEe5fYbyO9TSMCtaetSiLXxqU";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out var parsedParameters);

            Assert.IsTrue(result);
            Assert.AreEqual("photos,menu", parsedParameters.Get("vk_access_token_settings"));
        }

        [Test]
        [TestCase("89435HJGSDJH hj fg-932485%^&@#!%ty$^&324", TestName = "Wrong sign")]
        [TestCase("", TestName = "No sign")]
        [TestCase("ÅÍÎÏ˝ÓÔÒÚÆ☃社會科學院語學研究所👾 🙇 💁 🙅 🙆 🙋 🙎 🙍﷽", TestName = "Bad sign")]
        public void InvalidSignTest(string sign)
        {
            var parameters =
                $"vk_access_token_settings=photos,menu&vk_app_id=7605622&vk_are_notifications_enabled=1&vk_is_app_user=1&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147&sign={sign}";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out var parsedParameters);

            Assert.IsFalse(result);
            Assert.AreEqual("photos,menu", parsedParameters.Get("vk_access_token_settings"));
        }

        [Test]
        public void NoSignTest()
        {
            const string parameters =
                "vk_access_token_settings=photos,menu&vk_app_id=7605622&vk_are_notifications_enabled=1&vk_is_app_user=1&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out var parsedParameters);

            Assert.IsFalse(result);
            Assert.AreEqual("photos,menu", parsedParameters.Get("vk_access_token_settings"));
        }

        [Test]
        public void WrongParametersTest()
        {
            const string parameters =
                "vk_access_token_settings=photos,menu&vk_kjhasfgjhlsdjfakhldktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147&sign=BEZLyBbmG-CjEsunjTPEe5fYbyO9TSMCtaetSiLXxqU";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out var parsedParameters);

            Assert.IsFalse(result);
            Assert.AreEqual("photos,menu", parsedParameters.Get("vk_access_token_settings"));
        }

        [Test]
        public void NoParametersTest()
        {
            const string parameters = "sign=BEZLyBbmG-CjEsunjTPEe5fYbyO9TSMCtaetSiLXxqU";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void WrongSign()
        {
            const string parameters =
                "vk_access_token_settings=photos,menu&vk_app_id=7605622&vk_are_notifications_enabled=1&vk_is_app_user=1&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147&sign=BEZLyBbmG-CjEsunjTPEe5fYbyO9TSMCtaetSiLXxqU";
            const string secret = "sdjk89424579 &%^$%^$#@&#73c esdfg";

            var authenticator = new Authenticator(secret);
            var result = authenticator.Check(parameters, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void EmptyTest()
        {
            const string parameters = "";

            var authenticator = new Authenticator(Secret);
            var result = authenticator.Check(parameters, out _);

            Assert.IsFalse(result);
        }

        [Test]
        public void EmptyKeyTest()
        {
            const string parameters =
                "vk_access_token_settings=photos,menu&vk_app_id=7605622&vk_are_notifications_enabled=1&vk_is_app_user=1&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_ts=1618832593&vk_user_id=185117147&sign=BEZLyBbmG-CjEsunjTPEe5fYbyO9TSMCtaetSiLXxqU";
            const string secret = "";

            var authenticator = new Authenticator(secret);
            var result = authenticator.Check(parameters, out _);

            Assert.IsFalse(result);
        }
    }
}