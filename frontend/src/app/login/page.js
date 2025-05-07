'use client'
import Image from 'next/image'
import Link from 'next/link'

export default function LoginPage() {
    return (
        <div className="min-h-screen bg-gradient-to-br  flex items-center justify-center px-4">
            <div className="bg-white w-full max-w-5xl rounded-3xl overflow-hidden shadow-2xl flex flex-col md:flex-row transition-all duration-500">

                {/* Left - Form */}
                <div className="w-full md:w-1/2 p-10 flex flex-col justify-center">
                    <Link href="/" className="mb-6">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="24"
                            height="24"
                            viewBox="0 0 24 24"
                            fill="none"
                            stroke="currentColor"
                            strokeWidth="2"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            className="text-[#4D2C5E]  hover:text-purple-800 transition-colors"
                        >
                            <path d="m15 18-6-6 6-6" />
                        </svg>
                    </Link>

                    <div className="text-center md:text-left">
                        <p className="text-2xl font-extrabold text-gray-800 mb-2">Welcome Back 👋</p>
                        <p className="text-gray-500 mb-6">Please enter your credentials to login.</p>
                    </div>

                    <form className="space-y-5">
                        <input
                            type="text"
                            placeholder="Username"
                            className="w-full px-5 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-500 transition"
                        />
                        <input
                            type="password"
                            placeholder="Password"
                            className="w-full px-5 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-500 transition"
                        />
                        <div className="flex justify-end text-sm text-red-500 ">
                            <a href="#">Forgot Password?</a>
                        </div>
                        <Link
                            href="/dashboard"
                            className="block w-full text-center bg-gradient-to-r from-purple-600 to-indigo-500 hover:from-purple-700 hover:to-indigo-600 text-white font-semibold py-3 rounded-xl transition"
                        >
                            Login
                        </Link>
                    </form>

                    <div className="text-center text-sm text-gray-600 mt-6">
                        Don&apos;`t have an account?{" "}
                        <Link href="/signup" className="text-purple-600 hover:underline font-medium">
                            Sign up
                        </Link>
                    </div>
                </div>

                {/* Right - Illustration */}
                <div className="hidden md:flex md:w-1/2 bg-gradient-to-br from-[#4D2C5E] to-[#4D2C5E]  items-center justify-center p-8">
                    <div className="w-80 h-80 relative">
                        <Image
                            src="/images/login/login.svg"
                            alt="Login illustration"
                            width={400}
                            height={500}
                            className="object-contain"
                        />
                    </div>
                </div>

            </div>
        </div>
    )
}
