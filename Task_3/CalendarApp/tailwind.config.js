export default
{
    content:
    [
        "./index.html",
        "./src/**/*.{vue,js,ts,jsx,tsx}",
    ],

    darkMode: 'class',
    theme:
    {
        extend:
        {
            colors:
            {
                primary:
                {
                    50: '#f5f3ff',
                    100: '#ede9fe',
                    200: '#ddd6fe',
                    300: '#c4b5fd',
                    400: '#a78bfa',
                    500: '#8b5cf6',
                    600: '#7c3aed',
                    700: '#6d28d9',
                    800: '#5b21b6',
                    900: '#4c1d95',
                },

                accent:
                {
                    50: '#fdf4ff',
                    100: '#fae8ff',
                    200: '#f5d0fe',
                    300: '#f0abfc',
                    400: '#e879f9',
                    500: '#d946ef',
                    600: '#c026d3',
                    700: '#a21caf',
                    800: '#86198f',
                    900: '#701a75',
                }
            },

            animation:
            {
                'fade-in': 'fadeIn 0.5s ease-out',
                'slide-in': 'slideIn 0.5s ease-out',
                'glow': 'glow 2s infinite',
            },

            keyframes:
            {
                fadeIn:
                {
                    '0%': { opacity: '0', transform: 'translateY(10px)' },
                    '100%': { opacity: '1', transform: 'translateY(0)' },
                },

                slideIn:
                {
                    '0%': { opacity: '0', transform: 'translateX(-20px)' },
                    '100%': { opacity: '1', transform: 'translateX(0)' },
                },

                glow:
                {
                    '0%, 100%': { boxShadow: '0 0 20px rgba(139, 92, 246, 0.5)' },
                    '50%': { boxShadow: '0 0 30px rgba(139, 92, 246, 0.8)' },
                },
            },

            backdropBlur:
            {
                xs: '2px',
            }
        }
    },

    plugins: []
}