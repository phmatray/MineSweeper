/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.{razor,html,cshtml}',
    './wwwroot/index.html',
    './App.razor',
    './Pages/**/*.{razor,html}',
    './Components/**/*.{razor,html}',
    './Layout/**/*.{razor,html}',
    './Shared/**/*.{razor,html}'
  ],
  theme: {
    extend: {
      colors: {
        // Custom colors if needed
      },
      animation: {
        // Custom animations if needed
      }
    },
  },
  plugins: [],
}