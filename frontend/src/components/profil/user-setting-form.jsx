"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"

export function UserSettingsForm({ user }) {
    // Initialiser le state avec les données de l'utilisateur
    const [userDetails, setUserDetails] = useState(user || {})
    console.log('user stiing')
    console.log(userDetails)
    const [passwords, setPasswords] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    })

    // Mettre à jour le state quand les props changent
    useEffect(() => {
        setUserDetails(user || {})
    }, [user])

    const handleDetailsChange = (e) => {
        const { name, value } = e.target
        setUserDetails((prev) => ({ ...prev, [name]: value }))
    }

    const handlePasswordChange = (e) => {
        const { name, value } = e.target
        setPasswords((prev) => ({ ...prev, [name]: value }))
    }

    const handleDetailsSubmit = async (e) => {
        e.preventDefault()
        try {
            const response = await fetch(`http://localhost:3003/users${user.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userDetails),
            })
            if (!response.ok) throw new Error('Échec de la mise à jour')
            alert('Modifications sauvegardées !')
        } catch (error) {
            console.error('Erreur:', error)
        }
    }

    const handlePasswordSubmit = async (e) => {
        e.preventDefault()
        if (passwords.newPassword !== passwords.confirmPassword) {
            alert('Les mots de passe ne correspondent pas')
            return
        }

        try {
            const response = await fetch(`http://localhost:3003/users/${user.id}`, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    password: passwords.newPassword
                }),
            })
            if (!response.ok) throw new Error('Échec de la mise à jour')
            alert('Mot de passe modifié !')
            setPasswords({
                currentPassword: "",
                newPassword: "",
                confirmPassword: "",
            })
        } catch (error) {
            console.error('Erreur:', error)
        }
    }

    return (
        <Card className="border-0 shadow-lg bg-white profil-card">
            <CardHeader>
                <CardTitle className="text-2xl font-bold text-[#4a2a5a]">User Settings</CardTitle>
            </CardHeader>

            <CardContent className="space-y-8">
                {/* Details Section */}
                <form onSubmit={handleDetailsSubmit}>
                    <div className="space-y-6 details">
                        <h3 className="text-lg font-semibold text-[#4a2a5a]">Details</h3>
                        <div className="grid grid-cols-2 gap-4">
                            {/* Ajouter onChange à tous les inputs */}
                            {[
                                { label: "Name", name: "firstName" },
                                { label: "Last Name", name: "lastName" },
                                { label: "Email", name: "email", type: "email" },
                                { label: "Tel - Number", name: "phone" }
                            ].map((field) => (
                                <div className="space-y-1" key={field.name}>
                                    <Label className="text-sm font-medium text-gray-700">{field.label}</Label>
                                    <Input
                                        name={field.name}
                                        type={field.type || "text"}
                                        value={userDetails[field.name] || ""}
                                        onChange={handleDetailsChange}
                                        className="h-[50px] focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                    />
                                </div>
                            ))}
                        </div>
                        <div className="flex justify-start pt-4">
                            <Button type="submit" className="bg-[#6A1ACF] text-white hover:bg-[#7A1ACF] px-5 py-2 text-sm font-medium rounded-lg">
                                Save changes
                            </Button>
                        </div>
                    </div>
                </form>

                <Separator className="bg-gray-300 h-[1.5px]" />

                {/* Password Section */}
                <form onSubmit={handlePasswordSubmit}>
                    <div className="space-y-6 password">
                        <h3 className="text-lg font-semibold text-[#4a2a5a]">Password</h3>
                        <div className="space-y-4">
                            <div className="grid grid-cols-2 gap-4">
                                {[
                                    { label: "Current Password", name: "currentPassword" },
                                    { label: "New Password", name: "newPassword" },
                                    { label: "Confirm New Password", name: "confirmPassword" }
                                ].map((field) => (
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
                                ))}
                            </div>
                        </div>
                        <div className="flex items-center justify-between pt-4">
                            <Button type="submit" className="bg-[#6A1ACF] text-white hover:bg-[#7A1ACF] px-5 py-2 text-sm font-medium rounded-lg">
                                Save changes
                            </Button>
                            <Button variant="link" className="text-[#4a2a5a] hover:text-[#3a1a4a] p-0 text-sm font-medium">
                                Forgot your password?
                            </Button>
                        </div>
                    </div>
                </form>
            </CardContent>
        </Card>
    )
}