"use client";

import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Separator } from "@/components/ui/separator";

export function UserSettingsForm({ user,role }) {
    console.log('roe : ', role);
    // Initialiser le state avec les données de l'utilisateur
    const [passwords, setPasswords] = useState({
        oldPassword: "",
        newPassword: "",
        confirmPassword: "",
    });

    // Mettre à jour le state quand les props changent
    useEffect(() => {
        // Initialiser les mots de passe vides
        setPasswords({
            oldPassword: "",
            newPassword: "",
            confirmPassword: "",
        });
    }, [user]);

    const handlePasswordChange = (e) => {
        const { name, value } = e.target;
        setPasswords((prev) => ({ ...prev, [name]: value }));
    };

    const handlePasswordSubmit = async (e) => {
        e.preventDefault();

        // Vérification que les nouveaux mots de passe correspondent
        if (passwords.newPassword !== passwords.confirmPassword) {
            alert('Les mots de passe ne correspondent pas');
            return;
        }

        // Vérification que le mot de passe actuel est fourni
        if (!passwords.oldPassword) {
            alert('Le mot de passe actuel est requis');
            return;
        }

        try {
            const token = localStorage.getItem("accessToken");
            console.log(token);
            const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
            // Envoi de la requête pour modifier le mot de passe
            const response = await fetch(`${baseUrl}/api/Auth/resetPassword`, {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json", // ❗️ requis pour envoyer du JSON
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    email: user.email,
                    oldPassword: passwords.oldPassword,
                    newPassword: passwords.newPassword,
                }),
            });


            // Vérification de la réponse du serveur
            if (!response.ok) {
                const data = await response.json();
                throw new Error(data.message || 'Échec de la mise à jour du mot de passe');
            }

            // Si tout est ok, on réinitialise les champs de mot de passe
            alert('Mot de passe modifié avec succès!');
            setPasswords({
                oldPassword: "",
                newPassword: "",
                confirmPassword: "",
            });
        } catch (error) {
            console.error('Erreur:', error);
            alert(error.message);
        }
    };
    return (
        <Card className="border-0 shadow-lg bg-white profil-card">
            <CardHeader>
                <CardTitle className="text-2xl font-bold text-[#4a2a5a]">User Settings</CardTitle>
            </CardHeader>

            <CardContent className="space-y-8">
                {/* Details Section */}
                <div className="space-y-6 details">
                    <h3 className="text-lg font-semibold text-[#4a2a5a]">Details</h3>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Name</Label>
                            <Input
                                name="firstName"
                                value={user.firstName || ""}
                                className="h-[50px] focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                readOnly
                            />
                        </div>

                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Last Name</Label>
                            <Input
                                name="lastName"
                                value={user.lastName || ""}
                                className="h-[50px] focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                readOnly
                            />
                        </div>

                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Email</Label>
                            <Input
                                name="email"
                                type="email"
                                value={user.email || ""}
                                className="h-[50px] focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                readOnly
                            />
                        </div>

                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">
                               Filière
                            </Label>
                            <Input
                                name="filiere"
                                value="GINF2"
                                className="h-[50px] focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                readOnly
                            />
                        </div>
                    </div>
                </div>


                <Separator className="bg-gray-300 h-[1.5px]" />

                {/* Password Section */}
                <form onSubmit={handlePasswordSubmit}>
                    <div className="space-y-6 password">
                        <h3 className="text-lg font-semibold text-[#4a2a5a]">Password</h3>
                        <div className="space-y-4">
                            <div className="grid grid-cols-2 gap-4">
                                {[{ label: "Current Password", name: "oldPassword" },
                                    { label: "New Password", name: "newPassword" },
                                    { label: "Confirm New Password", name: "confirmPassword" }]
                                    .map((field) => (
                                        <div className="space-y-1" key={field.name}>
                                            <Label className="text-sm font-medium text-gray-700">{field.label}</Label>
                                            <Input
                                                type="password"
                                                name={field.name}
                                                value={passwords[field.name]}
                                                onChange={handlePasswordChange}
                                                className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                            />
                                        </div>
                                    ))
                                }
                            </div>
                        </div>
                        <div className="flex items-center justify-between pt-4">
                            <Button type="submit" className="bg-[#6A1ACF] text-white hover:bg-[#7A1ACF] px-5 py-2 text-sm font-medium rounded-lg">
                                Save changes
                            </Button>
                        </div>
                    </div>
                </form>
            </CardContent>
        </Card>
    );
}
