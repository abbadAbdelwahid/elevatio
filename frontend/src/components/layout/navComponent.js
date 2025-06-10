'use client';
import React, { useState } from 'react';
import Link from "next/link";
import Image from "next/image";

function NavComponent() {
    const [menuOpen, setMenuOpen] = useState(false);

    return (
        <nav className="bg-white dark:bg-[#4D2C5E] fixed w-full z-20 top-0 start-0 shadow-lg">
            <div className="container mx-auto px-6 py-4">
                <div className="flex items-center justify-between">
                    {/* Logo */}
                    <Link href="/frontend/public" className="flex items-center space-x-2">
                        <Image
                            src="https://flowbite.com/docs/images/logo.svg"
                            width={32}  // Obligatoire (correspond à w-8 en Tailwind)
                            height={32} // Obligatoire (correspond à h-8 en Tailwind)
                            alt="Flowbite Logo"
                            className="relative h-8 w-8 object-contain" // Ajout de 'relative' pour le positionnement
                        />
                        <span className="text-2xl font-bold bg-gradient-to-r from-purple-600 to-blue-500 bg-clip-text text-transparent">
                            Elevatio
                        </span>
                    </Link>

                    {/* Desktop Links */}
                    <div className="hidden md:flex items-center space-x-8">
                        <Link href="#" className="text-sm font-medium text-gray-600 dark:text-gray-200 hover:text-purple-600 dark:hover:text-purple-400 transition">
                            Home
                        </Link>
                        <Link href="#" className="text-sm font-medium text-gray-600 dark:text-gray-200 hover:text-purple-600 dark:hover:text-purple-400 transition">
                            About
                        </Link>
                        <Link href="#" className="text-sm font-medium text-gray-600 dark:text-gray-200 hover:text-purple-600 dark:hover:text-purple-400 transition">
                            Services
                        </Link>
                        <Link href="#" className="text-sm font-medium text-gray-600 dark:text-gray-200 hover:text-purple-600 dark:hover:text-purple-400 transition">
                            Contact
                        </Link>
                    </div>

                    {/* CTA Buttons - Desktop */}
                    <div className="hidden md:flex items-center space-x-4">
                        <Link href="#" className="text-sm font-medium text-purple-600 dark:text-purple-300 hover:bg-purple-50 dark:hover:bg-purple-900/20 rounded-full px-5 py-2.5 transition">
                            Sign Up
                        </Link>
                        <Link href="/login" className="text-sm font-medium bg-gradient-to-r from-purple-600 to-blue-500 text-white px-6 py-2.5 rounded-full hover:from-purple-700 hover:to-blue-600 transition">
                            Login
                        </Link>
                    </div>

                    {/* Mobile Menu Toggle */}
                    <button
                        onClick={() => setMenuOpen(!menuOpen)}
                        className="md:hidden p-2 rounded-lg text-gray-600 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-700 transition"
                        aria-label="Toggle menu"
                    >
                        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"/>
                        </svg>
                    </button>
                </div>

                {/* Mobile Menu */}
                {menuOpen && (
                    <div className="md:hidden mt-4">
                        <div className="flex flex-col space-y-4 py-4">
                            <Link href="#" className="text-gray-600 dark:text-gray-200">Home</Link>
                            <Link href="#" className="text-gray-600 dark:text-gray-200">About</Link>
                            <Link href="#" className="text-gray-600 dark:text-gray-200">Services</Link>
                            <Link href="#" className="text-gray-600 dark:text-gray-200">Contact</Link>
                            <div className="flex flex-col space-y-3 pt-4 border-t border-gray-200 dark:border-gray-700">
                                <Link href="#" className="text-purple-600 dark:text-purple-400">Sign Up</Link>
                                <Link href="/login" className="bg-purple-600 text-white py-2 px-4 rounded-full text-center">
                                    Login
                                </Link>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </nav>
    );
}

export default NavComponent;
