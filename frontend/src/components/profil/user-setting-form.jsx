"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"

export function UserSettingsForm() {
    const [userDetails, setUserDetails] = useState({
        firstName: "Pepito Rodrick",
        lastName: "Coronel Sifuentes",
        email: "pepito.c.sifuentes@uni.pe",
        phone: "+51 969 123 456",
    })

    const [passwords, setPasswords] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    })

    const handleDetailsChange = (e) => {
        const { name, value } = e.target
        setUserDetails((prev) => ({ ...prev, [name]: value }))
    }

    const handlePasswordChange = (e) => {
        const { name, value } = e.target
        setPasswords((prev) => ({ ...prev, [name]: value }))
    }

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
                                value={userDetails.firstName}
                                className="h-[50px]   focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                            />
                        </div>
                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Last Name</Label>
                            <Input
                                name="lastName"
                                value={userDetails.lastName}
                                className="  focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                            />
                        </div>
                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Email</Label>
                            <Input
                                name="email"
                                type="email"
                                value={userDetails.email}
                                className=" focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                            />
                        </div>
                        <div className="space-y-1">
                            <Label className="text-sm font-medium text-gray-700">Tel - Number</Label>
                            <Input
                                name="phone"
                                value={userDetails.phone}
                                className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                            />
                        </div>
                    </div>
                    <div className="flex justify-start pt-4">
                        <Button className="bg-[#6A1ACF] text-white hover:bg-[#7A1ACF] hover:bg-[#3a1a4a] px-5 py-2 text-sm font-medium rounded-lg">
                            Save changes
                        </Button>
                    </div>
                </div>

                <Separator className="bg-gray-300 h-[1.5px]" />

                {/* Password Section */}
                <div className="space-y-6 password">
                    <h3 className="text-lg font-semibold text-[#4a2a5a]">Password</h3>
                    <div className="space-y-4">
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-1">
                                <Label className="text-sm font-medium text-gray-700">Current Password</Label>
                                <Input
                                    type="password"
                                    placeholder="Put your password..."
                                    className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                />
                            </div>
                            <div className="space-y-1">
                                <Label className="text-sm font-medium text-gray-700">Confirm Password</Label>
                                <Input
                                    type="password"
                                    placeholder="Confirm password..."
                                    className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                />
                            </div>
                        </div>
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-1">
                                <Label className="text-sm font-medium text-gray-700">New Password</Label>
                                <Input
                                    type="password"
                                    placeholder="Put your new password..."
                                    className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                />
                            </div>
                            <div className="space-y-1">
                                <Label className="text-sm font-medium text-gray-700">Confirm New Password</Label>
                                <Input
                                    type="password"
                                    placeholder="Confirm new password..."
                                    className="border border-gray-300 rounded-sm focus:ring-1 focus:ring-[#4a2a5a] bg-white"
                                />
                            </div>
                        </div>
                    </div>
                    <div className="flex items-center justify-between pt-4">
                        <Button className="bg-[#6A1ACF] text-white hover:bg-[#7A1ACF] px-5 py-2 text-sm font-medium rounded-lg">
                            Save changes
                        </Button>
                        <Button variant="link" className="text-[#4a2a5a] hover:text-[#3a1a4a] p-0 text-sm font-medium">
                            Forgot your password?
                        </Button>

                    </div>
                </div>
            </CardContent>
        </Card>
    )
}