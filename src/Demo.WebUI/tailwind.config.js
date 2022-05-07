module.exports = {
  important: true,
  prefix: '',
  content: {
    content: ['./src/**/*.{html,ts}']
  },
  darkMode: 'class',
  theme: {
    extend: {}
  },
  variants: {
    extend: {}
  },
  plugins: [require('@tailwindcss/forms'), require('@tailwindcss/typography')]
};
