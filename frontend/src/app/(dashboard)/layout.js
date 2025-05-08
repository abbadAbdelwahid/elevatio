import "@/app/globals.css"
import { Sidebar } from "@/components/sidebar"
import { ThemeProvider } from "@/components/theme-provider"
import { Inter } from "next/font/google"

const inter = Inter({ subsets: ["latin"] })

export const metadata = {
    title: "CPS Student Dashboard",
    description: "Student portal for CPS",
}

export default function RootLayout({ children }) {
    return (
        <html lang="en">
        <body className={inter.className}>
        <ThemeProvider defaultTheme="light">
            <div className="flex min-h-screen">
                <Sidebar />
                <main className="flex-1">{children}</main>
            </div>
        </ThemeProvider>
        </body>
        </html>
    )
}
