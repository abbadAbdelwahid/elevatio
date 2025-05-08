"use client"

import { BookOpen, LogOut, Settings } from "lucide-react"
import Link from "next/link"
import { usePathname } from "next/navigation"

export function Sidebar() {
    const pathname = usePathname()

    const navigation = [
        { name: "Dashboard", href: "/dashboard", icon: BookOpen },
        { name: "Courses", href: "/courses", icon: BookOpen },
        { name: "Profil", href: "/profil", icon: Settings },
      ]

    return (
        <div className="flex h-screen w-[220px] flex-col bg-[#4a2a5a] text-white">
            <div className="flex  h-26 items-center justify-center border-b border-[#5a3a6a]">
                <Link href="/" className="flex items-center">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-white">
                        <BookOpen className="h-6 w-6 text-[#4a2a5a]" />
                    </div>
                </Link>
            </div>
            <nav className="flex-1 container space-y-2 px-4 py-8 mt-3">
                {navigation.map((item) => {
                    const isActive = pathname === item.href
                    return (
                        <Link
                            key={item.name}
                            href={item.href}
                            className={`flex items-center rounded-md px-4 py-3 text-lg font-medium ${
                                isActive ? "bg-white text-[#4a2a5a]" : "text-white hover:bg-[#5a3a6a]"
                            }`}
                        >
                          <h2>{item.name}</h2>
                        </Link>
                    )
                })}
            </nav>
            <div className="p-4">
                <Link href={'/'} className="flex w-full items-center rounded-md px-4 py-2 text-sm font-medium text-white hover:bg-[#5a3a6a]">
                    <LogOut className="mr-2 h-5 w-5" />
                    Logout
                </Link>
            </div>
        </div>
    )
}
