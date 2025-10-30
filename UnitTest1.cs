using Moq;
using System;
using System.Security.Policy;
using WinFormsApp4;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace TestProjectWinFormsApp4_1_
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("dssd")]
        [InlineData("password123")]
        [InlineData("hello world")]
        [InlineData("123456")]
        public void Test1(string password)
        {
            var hashr = new Form3();

            string resulthash = hashr.hashpqpass(password);


            Assert.NotNull(resulthash);
            Assert.NotEmpty(resulthash);
            Assert.Equal(64, resulthash.Length); // SHA256 hash всегда 64 символа
            Assert.Matches("^[a-f0-9]{64}$", resulthash);// Только hex символы
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Test2(string inalidpassword)
        {
            var hash = new Form3();
            var exception = Assert.Throws<Exception>(() => hash.hashpqpass(inalidpassword));

            Assert.Contains("Ошибка", exception.Message); 

        }


        [Fact]
        public void Test3()
        {
            var hash = new Form3();
            string password = "dsdsdd";
            string hash1 = hash.hashpqpass(password);
            string hash2 = hash.hashpqpass(password);
            Assert.Equal(hash1,hash2);
        }

        [Fact]
        public void Test4()
        {
            var hash = new Form3();
            string password = "dsdsdd";
            string password2 = "dsdsdsd";
            string hash1 = hash.hashpqpass(password);
            string hash2 = hash.hashpqpass(password2);
            Assert.NotEqual(hash1, hash2);
        }


        [Fact]
        public async Task Test5()
        {
            var valutelocaltest = new smenadannix();
            var result = await valutelocaltest.valutelocal();
            Assert.True(result);
        }

        [Fact]
        public async Task Test6()
        {
            var valutelocaltest = new smenadannix();

            var exception = await Record.ExceptionAsync(async () => await valutelocaltest.valutelocal());

            Assert.Null(exception);
        }

        [Fact]
        public async Task Test7()
        {
            var valutelocaltest = new smenadannix();

            try
            {
                await valutelocaltest.valutelocal();
            }
            catch (Exception ex) when (ex.Message.Contains("Не удается создать директорию"))
            {
                Assert.Fail($"Не ожидалось исключение: {ex.Message}");
            }
            catch
            {
                Assert.Fail("Произошло непредвиденное исключение");
            }
        }

        [Fact]
        public async Task Test8()
        {
            var valutelocaltest = new smenadannix();

            var result = await valutelocaltest.valutelocal();

            Assert.NotNull(result);
        }
    }
}