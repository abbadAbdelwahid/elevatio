"use client"

import { BookOpen, LogOut } from "lucide-react"
import Link from "next/link"
import {usePathname, useRouter} from "next/navigation"
import { useEffect, useMemo, useState } from "react"
import Image from "next/image"
import { getRoleFromCookie } from "@/lib/utils"

export function Sidebar() {
    const pathname = usePathname()
    const [role, setRole] = useState('')
    const router = useRouter()
    const handleLogout = () => {
        localStorage.removeItem("accessToken")
        document.cookie = "role=; Max-Age=0; path=/"
        document.cookie = "userId=; Max-Age=0; path=/"
        router.push("/") // Redirige vers la page d'accueil
    }
    useEffect(() => {
        const roleFromCookie = getRoleFromCookie()
        // console.log("Role from cookie:", roleFromCookie)
        setRole(roleFromCookie)
    }, [])

    const navigation = useMemo(() => {
        const allLinks = [
            // student routes
            { name: "Dashboard", href: "/etu/dashboard", icon: "/images/sidebar/dashboard-icon.svg", roles: ["Etudiant"] },
            { name: "Courses", href: "/etu/courses", icon: "/images/sidebar/coursesIcon.svg", roles: ["Etudiant"] },
            { name: "Planning", href: "/etu/edt", icon: "/images/sidebar/edt.svg", roles: ["Etudiant"] },
            { name: "Forms", href: "/etu/forms", icon: "/images/sidebar/form.svg", roles: ["Etudiant"] },

            // admin routes
            { name: "Dashboard", href: "/admin/dashboard", icon: "/images/sidebar/dashboard-icon.svg", roles: [ "Admin"] },
            { name: "Professors", href: "/admin/profs", icon: "/images/sidebar/profs.svg", roles: ["Admin"] },
            { name: "Course", href: "/admin/courses", icon: "/images/sidebar/coursesIcon.svg", roles: ["Admin"] },
            { name: "Planning", href: "/admin/edt", icon: "/images/sidebar/edt.svg", roles: ["Admin"] },
            { name: "Forms", href: "/admin/forms", icon: "/images/sidebar/form.svg", roles: ["Admin"] },
            //prof routes
            { name: "Dashboard", href: "/prof/dashboard", icon: "/images/sidebar/dashboard-icon.svg", roles: ["Enseignant"] },
            { name: "Students", href: "/prof/students", icon: "/images/sidebar/profs.svg", roles: ["Enseignant"] },
            { name: "Course", href: "/prof/courses", icon: "/images/sidebar/coursesIcon.svg", roles: ["Enseignant"] },
            { name: "Planning", href: "/prof/edt", icon: "/images/sidebar/edt.svg", roles: ["Enseignant"] },
            { name: "Forms", href: "/prof/forms", icon: "/images/sidebar/form.svg", roles: ["Enseignant"] },
            // common routes
            { name: "Profil", href: "/profil", icon: "/images/sidebar/profilIcon.svg", roles: ["Etudiant", "Admin","Enseignant"] },
        ]

        if (!role) return []
        return allLinks.filter(link => link.roles.includes(role))
    }, [role])

    return (
        <div className="flex h-screen w-[220px] flex-col bg-[#4a2a5a] text-white">
            <div className="flex h-26 items-center justify-center border-b border-[#5a3a6a]">
                <Link  href={role === "admin" ? "/admin/dashboard" : "/etu/dashboard"} className="flex items-center">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-white">
                        <BookOpen className="h-6 w-6 text-[#4a2a5a]" />
                    </div>
                </Link>
            </div>

            <nav className="flex-1 container space-y-3 px-4 py-8 mt-3">
                {navigation.map((item) => {
                    const isActive = pathname === item.href
                    return (
                        <div
                            key={item.name}
                            className={`flex items-center space-x-3 rounded-md ps-5 py-3 text-md font-medium ${
                                isActive ? "bg-white text-[#4a2a5a]" : "text-white hover:bg-[#5a3a6a]"
                            }`}
                        >
                            <Image src={item.icon} alt="Icon" width={22} height={22} />
                            <Link href={item.href}>
                                <h3>{item.name}</h3>
                            </Link>
                        </div>
                    )
                })}
            </nav>

                <div className="p-4 mb-5">
                    <button
                        onClick={handleLogout}
                        className="flex w-full items-center rounded-md px-4 py-2 text-sm font-medium text-white hover:bg-[#5a3a6a]"
                    >
                        <LogOut className="mr-2 h-5 w-5" />
                        Logout
                    </button>
            </div>
        </div>
    )
}
