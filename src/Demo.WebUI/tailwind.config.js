module.exports = {
  important: true,
  prefix: '',
  content: ['./src/**/*.{html,ts}'],
  darkMode: 'class',
  theme: {
    extend: {}
  },
  variants: {
    extend: {}
  },
  corePlugins: {
    preflight: false
  },
  plugins: [
    require('@tailwindcss/forms')({ strategy: 'class' }),
    require('@tailwindcss/typography')
  ]
};
