export class CreateCarReportDto {
  id!: number | null;
  customerName!: string;
  userId!: number;
  licensePlate!: string;
  reason!: string;
  content!: string;
}
