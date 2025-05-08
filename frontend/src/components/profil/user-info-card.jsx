import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

export function UserInfoCard() {
    return (
        <Card className="border-0 bg-white">
            <CardHeader>
                <CardTitle className="text-lg font-semibold text-[#4a2a5a]">Information</CardTitle>
            </CardHeader>
            <CardContent className="space-y-6 ms-3">
                <div className="flex space-x-4">
                    <h3 className="text-md font-semibold">Name:</h3>
                    <p className="text-sm text-muted-foreground">Name, Last Name</p>
                </div>
                <div className="flex space-x-4">
                    <p className="text-md font-semibold">Email:</p>
                    <p className="text-sm text-muted-foreground">user@gmail.com</p>
                </div>
                <div className="flex space-x-4">
                    <p className="text-md font-semibold">Tel:</p>
                    <p className="text-sm text-muted-foreground">+51 966 696 123</p>
                </div>
            </CardContent>
        </Card>
    )
}
