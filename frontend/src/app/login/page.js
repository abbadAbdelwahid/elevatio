'use client'
// Add this import
import Image from 'next/image'
import Link from 'next/link'
import { useState } from "react";
import { useRouter } from "next/navigation";
import { jwtDecode } from "jwt-decode";

export default function LoginPage() {
    const router = useRouter();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [errors, setErrors] = useState({});
    const [loading, setLoading] = useState(false);

    const validateEmail = (value) => {
        const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return pattern.test(value);
    };



    const handleSubmit = async (e) => {
        e.preventDefault();

        const newErrors = {};
        if (!email.trim()) {
            newErrors.email = "L'adresse email est requise.";
        } else if (!validateEmail(email)) {
            newErrors.email = "Adresse email invalide.";
        }

        if (!password.trim()) {
            newErrors.password = "Le mot de passe est requis.";
        }

        setErrors(newErrors);
        if (Object.keys(newErrors).length > 0) return;

        setLoading(true);

        try {
            const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
            const response = await fetch(`${baseUrl}/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ email, password }),
            });

            const data = await response.json();
            console.log(data); // Log pour vérifier la réponse

            if (response.ok) {
                console.log("I received a response!");

                // Vérifie si le token a un préfixe "Bearer"
                const accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiMTIzNDU2Nzg5Iiwicm9sZSI6InN0dWRlbnQiLCJpYXQiOjE2MTIyMzQyNjAsImV4cGlyZWRfc3RhcnRfdGltZSI6MTYxMjIzNDI2MH0.jZoNB2DP61ZZGbVQQRLmczcZJbqFlTxTBKTkyS4HD7k";  // Supprimer le préfixe "Bearer "
                console.log("AccessToken:", accessToken); // Log pour vérifier le token sans "Bearer"

                try {
                    // Décoder le token
                    const decoded = jwtDecode(accessToken);
                    console.log("Decoded JWT:", decoded);  // Vérifie que le token est bien décodé

                    const role = decoded.role;  // Extraction du rôle du payload
                    console.log("Role:", role);  // Log pour vérifier le rôle extrait du token

                    // Stocker le token dans localStorage
                    localStorage.setItem("accessToken", accessToken);

                    // Enregistrer le rôle dans un cookie
                    document.cookie = `role=${role}; path=/; secure; samesite=strict`;

                    // Redirection selon le rôle
                    if (role === "admin") {
                        router.push("/admin/dashboard");
                    } else {
                        router.push("/etu/dashboard");
                    }
                } catch (error) {
                    console.error("Error decoding JWT:", error);
                    setErrors({ form: "Erreur lors du décodage du token." });
                }

            } else {
                setErrors({ form: data.message || "Erreur de connexion." });
            }
        } catch (error) {
            console.error("Error during login:", error);
            setErrors({ form: "Erreur serveur. Réessayez plus tard." });
        } finally {
            setLoading(false);
        }
    };




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

                    <form onSubmit={handleSubmit} className="space-y-5">
                        {errors.form && (
                            <p className="text-red-500 text-sm">{errors.form}</p>
                        )}

                        <div>
                            <input
                                type="email"
                                placeholder="Email"
                                className="w-full px-5 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-500 transition"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                            />
                            {errors.email && (
                                <p className="text-red-500 text-sm mt-1">{errors.email}</p>
                            )}
                        </div>

                        <div>
                            <input
                                type="password"
                                placeholder="Password"
                                className="w-full px-5 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-500 transition"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                            {errors.password && (
                                <p className="text-red-500 text-sm mt-1">{errors.password}</p>
                            )}
                        </div>

                        <div className="flex justify-end text-sm text-red-500">
                            <a href="#">Mot de passe oublié ?</a>
                        </div>

                        <button
                            type="submit"
                            disabled={loading}
                            className="block w-full text-center bg-gradient-to-r from-purple-600 to-indigo-500 hover:from-purple-700 hover:to-indigo-600 text-white font-semibold py-3 rounded-xl transition"
                        >
                            {loading ? "Connexion..." : "Connexion"}
                        </button>
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
