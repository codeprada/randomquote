using System;

namespace RandomQuote.Data
{
    public class QuoteResponse
    {
        public Quote Quote { get; set; }
    }

    public class Quote
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
    }
}
