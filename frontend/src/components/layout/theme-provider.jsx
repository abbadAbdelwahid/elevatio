"use client"

import { createContext, useContext, useEffect, useState } from "react"

const ThemeProviderContext = createContext({})

export function ThemeProvider({ children, defaultTheme = "system", storageKey = "theme", ...props }) {
    const [theme, setTheme] = useState(defaultTheme)

    useEffect(() => {
        const root = window.document.documentElement

        // Remove old theme class
        root.classList.remove("light", "dark")

        // Add new theme class
        if (theme === "system") {
            const systemTheme = window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light"
            root.classList.add(systemTheme)
            return
        }

        root.classList.add(theme)
    }, [theme])

    const value = {
        theme,
        setTheme: (newTheme) => {
            setTheme(newTheme)
            // Save to localStorage
            localStorage.setItem(storageKey, newTheme)
        },
    }

    return (
        <ThemeProviderContext.Provider {...props} value={value}>
            {children}
        </ThemeProviderContext.Provider>
    )
}

export const useTheme = () => {
    const context = useContext(ThemeProviderContext)
    if (context === undefined) throw new Error("useTheme must be used within a ThemeProvider")
    return context
}
