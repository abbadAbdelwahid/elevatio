import { Inter } from "next/font/google"
import "./globals.css"

// Configuration modifiée de la police avec fallback
const inter = Inter({
    subsets: ["latin"],
    weight: ["400", "500", "700"], // Ajout des poids nécessaires
    display: "swap", // Meilleure performance de chargement
    fallback: ["system-ui", "Arial"] // Fallback système
})

export const metadata = {
    title: "QualiTrack - Enhance the Quality of Higher Education",
    description:
        "A smart platform to manage, automate, and analyze training evaluations in universities and engineering schools.",
}

export default function RootLayout({ children }) {
    return (
        <html lang="en" suppressHydrationWarning={true}>
        <head>
            {/* Préchargement de la police */}
            <link
                rel="preload"
                href="/fonts/inter.ttf"
                as="font"
                type="font/woff2"
                crossOrigin="anonymous"
            />
        </head>
        <body className={inter.className}>
        {children}
        </body>
        </html>
    )
}