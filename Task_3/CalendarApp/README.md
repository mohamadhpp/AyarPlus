# Persian Triple Calendar - Desktop Application

A modern, minimal desktop calendar application built with Vue.js and Electron, featuring Persian (Jalali), Hijri (Islamic), and Gregorian calendar systems with smart reminders and beautiful glassmorphism UI.

## Features

### Triple Calendar System
- **Persian Calendar (Jalali)**: Complete solar calendar with Iranian holidays
- **Hijri Calendar (Islamic)**: Lunar Islamic calendar
- **Gregorian Calendar**: Standard international calendar

### Smart Date Conversion
- Real-time conversion between all three calendars
- Select a date from any calendar and see equivalents
- Display weekday names in Persian
- User-friendly interface

### Intelligent Reminder System
- Create personal reminders with date and time
- In-app notifications with animations
- Native desktop notifications
- Automatic checking every minute
- Badge counter for active reminders

### Modern Minimal Design
- Clean glassmorphism UI design
- Smooth micro-animations
- Purple gradient color palette
- Completely different from existing solutions
- Optimized user experience

### Dark & Light Themes
- Full support for dark and light themes
- Automatic settings persistence
- Instant theme switching
- Smooth transitions

### Advanced Settings
- Default calendar selection
- Notification management
- Default reminder time
- Holiday display toggle

## Technology Stack

- **Vue.js 3** - Composition API
- **Electron 32** - Desktop runtime
- **Vite 6** - Fast build tool
- **Pinia 2** - State management
- **moment-jalaali** - Persian calendar library
- **moment-hijri** - Hijri calendar library
- **CSS3** - Modern styling with variables

## Quick Start

### Prerequisites
- Node.js v16 or higher
- npm or yarn

### Installation

**Automatic (Windows):**
```bash
install-and-run.bat
```

**Automatic (Linux/Mac):**
```bash
chmod +x install-and-run.sh
./install-and-run.sh
```

**Manual:**
```bash
# Install dependencies
npm install

# Run development server
npm run electron:dev
```

## Documentation

- **[START_HERE.md](./START_HERE.md)** - Quick start guide
- **[QUICK_START.md](./QUICK_START.md)** - 5-minute tutorial
- **[INSTALLATION_GUIDE.md](./INSTALLATION_GUIDE.md)** - Complete installation guide
- **[FEATURES.md](./FEATURES.md)** - Creative features documentation
- **[API_DOCS.md](./API_DOCS.md)** - Complete API documentation
- **[PROJECT_SUMMARY.md](./PROJECT_SUMMARY.md)** - Project summary

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl + Shift + T` | Toggle theme (light/dark) |
| `Ctrl + R` | Open reminders |
| `Ctrl + ,` | Open settings |
| `Escape` | Close modals |

## Project Structure

```
CalendarApp/
├── src/
│   ├── components/       # Vue components
│   ├── stores/          # Pinia stores
│   ├── composables/     # Reusable logic
│   ├── App.vue          # Main component
│   ├── main.js          # Entry point
│   └── style.css        # Global styles
├── electron/
│   ├── main.js          # Electron main process
│   └── preload.js       # Preload script
├── public/
│   └── icon.svg         # App icon
└── docs/               # Documentation
```

## NPM Scripts

```bash
# Development
npm run electron:dev    # Run desktop app
npm run dev             # Run Vite only

# Production
npm run build           # Build files
npm run electron:build  # Build desktop app
npm run preview         # Preview build
```

## Highlights

- Complete triple calendar system
- Smart date conversion
- Reminder system with notifications
- Dark/Light theme with persistence
- Minimal modern UI
- Keyboard shortcuts
- Responsive design
- RTL support
- Clean architecture
- Comprehensive documentation

## Project Stats

- **Files**: 27+ files
- **Lines of Code**: ~5,100+ lines
- **Components**: 5 Vue components
- **Documentation**: 9 complete files
- **Features**: 15+ additional features

## Contributing

This project was created as a technical assessment for AyarPlus and demonstrates:

- Desktop app development with Electron
- Vue.js 3 Composition API
- State management with Pinia
- Modern UI/UX design
- Working with multiple calendar systems
- Notification systems
- Local storage management
- Dynamic theming

## License

This project is created for demonstration purposes only and has no commercial use.

## Author

Built for **AyarPlus**

---

**Note**: This project follows Clean Code principles, performance optimization, and best practices.

## Get Started Now!

```bash
npm run electron:dev
```

Enjoy the Triple Calendar App!
