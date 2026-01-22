
This document explains how AI tools were used during development and how their output was evaluated and corrected.

---

## AI Tools Used

- **ChatGPT**
  - Architecture brainstorming
  - Parsing logic suggestions
  - Error-handling patterns
- **GitHub Copilot**
  - Boilerplate generation
  - Async/await HTTP calls
  - Angular service and component scaffolding

AI tools were treated as **assistants**, not authorities. All generated code was reviewed and adapted.

---

## Most Helpful Prompts

### 1. Date Parsing in C#
> “Parse multiple date formats in C# safely and detect invalid dates without throwing exceptions.”

This helped identify:
- `DateTime.TryParseExact`
- Using multiple formats
- Avoiding exceptions during parsing

---

### 2. Open-Meteo API Integration
> “Design a clean .NET service class to call the Open-Meteo historical weather API using HttpClient.”

This provided:
- A clear service-based approach
- Async HTTP patterns
- DTO separation

---

### 3. Angular UI Data Fetching
> “Angular component example showing loading state, error handling, and table rendering from an API.”

This helped structure:
- Loading spinner logic
- Error messaging
- Clean separation between service and component

---

## Example of an AI Suggestion That Was Wrong

**Suggestion:**  
ChatGPT initially suggested using `DateTime.Parse()` directly for date parsing.

**Problem:**  
- `DateTime.Parse()` throws exceptions on invalid input
- This violated the requirement to handle invalid dates gracefully

**How I Detected It:**  
- Manual review against requirements
- Testing with `April 31, 2022` caused runtime exceptions

**Correction:**  
- Replaced with `DateTime.TryParseExact()` and explicit validation
- Invalid dates are now captured with error messages instead of crashing

---

## Code Written Manually (Not AI-Generated)

Some parts were intentionally written without AI assistance:

- **Overall solution structure**
  - Folder layout
  - Separation of concerns
- **Error model and API response shape**
  - Ensuring per-date status reporting
- **Caching logic**
  - File existence checks before API calls
- **Final UI behavior**
  - Sorting and interaction logic tailored to the requirements

These decisions were made to ensure clarity, correctness, and alignment with the exercise goals.

---

## Reflection

AI tools significantly accelerated development, especially for:
- Boilerplate
- Repetitive patterns
- Cross-checking best practices

However:
- All AI output required validation
- Edge cases and requirements still needed human judgment
- The best results came from **iterative prompting + manual refinement**

This mirrors how I would use AI in a real production environment.

---
