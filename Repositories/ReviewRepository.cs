using Food_Scape.Models;

namespace Food_Scape.Repositories
{
    
    public class ReviewRepository
    {
        private FoodScapeContext _foodScapeContext;
        public ReviewRepository(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }
        // Method that retrieves all reviews from the database and returns a list of Review objects
        public List<Review> GetAllReviews()
        {
            return _foodScapeContext.Reviews.OrderByDescending(r => r.Rating).ToList();
        }

        // Method that retrieves a single review by ID from the database and returns a Review object
        public Review? GetReviewById(int id)
        {
            return _foodScapeContext.Reviews.Where(r => r.ReviewId == id).FirstOrDefault();
        }

        // Method that retrieves a single review by user ID from the database and returns a Review object
        public Review? GetUserReview(int userId)
        {
            return _foodScapeContext.Reviews.Where(r => r.UserId == userId).FirstOrDefault();
        }

        // Method that creates a new review in the database based on a Review object passed as a parameter
        public void CreateReview(Review review)
        {
            _foodScapeContext.Reviews.Add(review);
            _foodScapeContext.SaveChanges();
        }

        // Method that updates an existing review in the database based on a Review object passed as a parameter
        public void UpdateReview(Review review)
        {
            _foodScapeContext.Reviews.Update(review);
            _foodScapeContext.SaveChanges();
        }

        // Method that deletes an existing review from the database based on its ID
        public void DeleteReview(int id)
        {
            Review? review = GetReviewById(id);
            if(review!= null)
            {
                _foodScapeContext.Reviews.Remove(review);
                _foodScapeContext.SaveChanges();
            }

        }

        // Method that retrieves a collection of the top 3 highest rated reviews from the database and returns an IEnumerable of Review objects
        public IEnumerable<Review> GetReviewTestemonials()
        {
            var reviews = _foodScapeContext.Reviews
                .Where(r => r.Rating >= 4) // filter by rating
                .OrderByDescending(r => r.Rating) // sort by rating in descending order
                .Take(3); // take the top 3 highest rated reviews

            return reviews;
        }
    }
}

