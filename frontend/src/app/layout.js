import { Inter } from "next/font/google"
import "./globals.css"

const inter = Inter({ subsets: ["latin"] })

export const metadata = {
    title: "QualiTrack - Enhance the Quality of Higher Education",
    description:
        "A smart platform to manage, automate, and analyze training evaluations in universities and engineering schools.",
}

export default function RootLayout({ children }) {
    return (
        <html lang="en">
        <body className={inter.className}>{children}</body>
        </html>
    )
}
