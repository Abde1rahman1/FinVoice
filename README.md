# FinVoice

FinVoice is a financial management application that allows users to track their expenses and manage their budgets. The application provides a set of APIs to handle user authentication, expense tracking (including voice-based input), budget management, and financial analysis.

## Key Features

- **User Authentication**: Secure registration and login functionality.
- **Expense Tracking**: Add expenses manually or through voice input.
- **Budget Management**: Set and track monthly budgets.
- **Financial Analysis**: Get insights into spending habits.

## API Endpoints

Here is a list of the available API endpoints:

### Auth Controller

- **POST /api/Auth/register**: Register a new user.
- **POST /api/Auth/login**: Log in an existing user.
- **POST /api/Auth/confirm-email**: Confirm a user's email address.
- **POST /api/Auth/resend-confirmation-email**: Resend the confirmation email.

### Audio Expenses Controller

- **POST /api/AudioExpenses/analyze**: Analyze an audio file to extract and save expense information.

### Manual Expense Controller

- **POST /api/ManualExpense/add-expense**: Add a new expense manually.
- **POST /api/ManualExpense/add-ai-expense**: Add a new expense using AI for categorization.

### Budget Controller

- **POST /api/Budget/add**: Add a new budget for the user.
- **GET /api/Budget/get**: Get the user's current budget.
- **GET /api/Budget/compare**: Compare the user's budget with their expenses.

### Analysis Controller

- **GET /api/Analysis/spending-insights**: Get insights into the user's spending habits.