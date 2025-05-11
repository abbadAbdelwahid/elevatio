
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

export function RecentEvaluations({evaluations}) {

    return (
        <Card className={' border-0'}>
            <CardHeader>
                <CardTitle className="text-lg font-bold uppercase tracking-widest text-[#0E2C75]">Recent Evaluation</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
                {evaluations.map((evaluation) => (
                    <div key={evaluation.id} className={`flex items-center justify-between rounded-lg ${evaluation.bgColor} p-4`}>
                        <div>
                            <h3 className="font-medium">{evaluation.course}</h3>
                            <p className="text-sm text-muted-foreground">{evaluation.professor}</p>
                        </div>
                        <div className="text-right">
                            <p className="font-medium">{evaluation.time}</p>
                        </div>
                    </div>
                ))}
            </CardContent>
        </Card>
    )
}
