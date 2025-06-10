'use client'
import { UserSettingsForm } from "@/components/profil/user-setting-form"
import { UserProfileCard } from "@/components/profil/user-profil-card"
import { UserInfoCard } from "@/components/profil/user-info-card"
import { SearchHeader } from "@/components/layout/search-header";
import { useEffect, useState } from "react";
import {getRoleFromCookie, getUserIdFromCookie} from "@/lib/utils";

export default function SettingsPage() {
    const [userDetails, setUserDetails] = useState({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        image: "",
    });
    const [role, setRole] = useState({});

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
                // Si l'API nécessite un token d'authentification (par exemple dans un en-tête Authorization)
                const token = localStorage.getItem("accessToken"); // ou une autre méthode pour récupérer le token
                const roleFromCookie = getRoleFromCookie()
                const userIdFromCookie=getUserIdFromCookie()
                setRole(roleFromCookie)
                console.log('userId: ',userIdFromCookie)
                console.log("Role from cookie:", roleFromCookie)
                const test=`${baseUrl}/${roleFromCookie}s/me`
                console.log(test)
                let res=undefined;
                if(roleFromCookie==='Admin'){
                    res = await fetch(`${baseUrl}/api/auth/Admin/me`, {
                        method: "GET",
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
                }else{
                     res = await fetch(`${baseUrl}/api/${roleFromCookie}s/me`, {
                        method: "GET",
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
                }


                // Vérifier si la réponse est correcte avant de la traiter
                if (!res.ok) {
                    throw new Error("Erreur de récupération des données");
                }

                const data = await res.json();
                console.log(data);
                // Si la réponse est un tableau, on prend le premier élément, sinon on l'utilise directement
                setUserDetails(data[0] || data); // Si c'est un tableau, on prend le premier élément

            } catch (error) {
                console.error("Erreur de récupération des données", error);
            }
        };

        fetchUserData();
    }, []);

    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            <div className="container flex-1 p-8">
                <SearchHeader />

                <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                    <div className="space-y-6 md:col-span-1">
                        <UserProfileCard user={userDetails} />
                        <UserInfoCard user={userDetails} />
                    </div>

                    <div className="md:col-span-2">
                        <UserSettingsForm user={userDetails} role={role} />
                    </div>
                </div>
            </div>
        </div>
    );
}
