import "@/app/globals.css"
import { Sidebar } from "@/components/layout/sidebar"
import { ThemeProvider } from "@/components/layout/theme-provider"


export const metadata = {
    title: "CPS Student Dashboard",
    description: "Student portal for CPS",
}

export default function RootLayout({ children }) {
    return (

        <ThemeProvider defaultTheme="light">
            <div className="flex min-h-screen">
                <Sidebar />
                <main className="flex-1">{children}</main>
            </div>
        </ThemeProvider>
    )
}
