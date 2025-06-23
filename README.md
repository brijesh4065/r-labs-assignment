# RaftLabs - .NET Developer Assignment 🚀

This repository contains my submission for the **RaftLabs .NET Developer Assignment**.  

The solution demonstrates:

- ✅ A **.NET Core Console Application**
- ✅ A modular **C# Class Library** architecture
- ✅ Integration with the public [ReqRes API](https://reqres.in/)
- ✅ Clean code structure, async programming, and proper error handling

---

## 📂 Project Structure

r-labs-assignment/
├── PracticalTest.Client       # Entry point console app to run the solution  
├── PracticalTest.Domain       # DTOs/Models used across the solution  
├── PracticalTest.External     # API client for interacting with ReqRes API  
├── PracticalTest.Service      # Service layer to encapsulate business logic  
└── PracticalTest.Tests        # Unit test project (XUnit)  

---

## ⚙️ How to Run the Solution

### 🖥️ Prerequisites

- IDE: Visual Studio 2022+

---

### ▶️ Running the Console Application

1. **Clone the repository:**

   git clone https://github.com/brijesh4065/r-labs-assignment.git  
   cd r-labs-assignment

2. **Build the solution:**

   dotnet build

3. **Run the console app (PracticalTest.Client):**

   dotnet run --project PracticalTest.Client

---

### 🧪 Running Unit Tests

To execute unit tests

## ✅ Highlights

- `HttpClient` usage for calling external APIs
- `async/await` pattern with proper exception handling
- Models mapped from JSON responses (`id`, `email`, `first_name`, `last_name`)
- Pagination handled automatically for listing all users
- Clean, layered architecture

---

## 📄 Future Improvements (Bonus Points - As Suggested)

- [ ] Add **caching** layer for repeated API responses  
- [ ] Implement **retry logic** using **Polly** for transient failures  
- [ ] Improve configuration using **Options Pattern**  
- [ ] Extend architecture towards **Clean Architecture** principles  

---

## 👨‍💻 Author

**Brijesh Patel**  
🔗 https://github.com/brijesh4065

---

> Feel free to fork, explore, or provide feedback on this solution!

