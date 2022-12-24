using System.Net;
using System.Text;
using BookLibrary.Domain.Dto;
using FluentAssertions;
using IntegrationTestCase.Setup;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTestCase
{
    public class BookApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string BOOK_NAME = "My Book";
        private const string BOOK_AUTHOR_NAME = "Author Name";
        public BookApiIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostBookShouldReturn_Saved_Book_Results()
        {
            var bookDto = CreateBookDto(BOOK_NAME, BOOK_AUTHOR_NAME);

            var response = await PostBook(bookDto);

            var responseString = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<BookDto>(responseString);

            dto.Should().NotBeNull();
            dto.Id.Should().NotBeEmpty();
            dto.Name.Should().Be(BOOK_NAME);
            dto.AuthorName.Should().Be(BOOK_AUTHOR_NAME);
        }

        [Fact]
        public void PostBook_Should_Throw_BadHttpRequestException_When_BookName_IsEmpty()
        {
            var bookDto = CreateBookDto("", BOOK_AUTHOR_NAME);
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/books");
            postRequest.Content = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");

            _ = Assert.ThrowsAsync<BadHttpRequestException>(() => _client.SendAsync(postRequest));
        }


        [Fact]
        public void PostBook_Should_Throw_BadHttpRequestException_When_Book_Author_Name_IsEmpty()
        {
            var bookDto = CreateBookDto("test", "");
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/books");
            postRequest.Content = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");

            _ = Assert.ThrowsAsync<BadHttpRequestException>(() => _client.SendAsync(postRequest));
        }


        [Fact]
        public async Task BookGetById_ShouldReturn_Book()
        {
            BookDto book = CreateBookDto(BOOK_NAME, BOOK_AUTHOR_NAME);
            HttpResponseMessage response = await PostBook(book);

            var responseString = await response.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<BookDto>(responseString);

            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/books/{dto?.Id}");

            response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BookDto>(responseString);
            result.Should().NotBeNull();
            result?.Id.Should().Be(dto?.Id.ToString());
            result?.Name.Should().Be(BOOK_NAME);
            result?.AuthorName.Should().Be(BOOK_AUTHOR_NAME);
        }

        [Fact]
        public async Task GetAllBook_Should_Return_All_Books()
        {
            BookDto book = CreateBookDto(BOOK_NAME, BOOK_AUTHOR_NAME);
            await PostBook(book);

            BookDto book1 = CreateBookDto("My Book1", "My Author1");
            await PostBook(book1);
            

            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/books");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IReadOnlyList<BookDto>>(responseString);
            result.Should().NotBeEmpty();
            result.Should().HaveCount(4);
            result[0].Name.Should().Be(BOOK_NAME);
            result[0].AuthorName.Should().Be(BOOK_AUTHOR_NAME);
        }

        private async Task<HttpResponseMessage> PostBook(BookDto bookDto)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/books");
            postRequest.Content = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static BookDto CreateBookDto(string name, string authorName)
        {
            return new BookDto()
            {
                Name = name,
                AuthorName = authorName
            };
        }
    }
}