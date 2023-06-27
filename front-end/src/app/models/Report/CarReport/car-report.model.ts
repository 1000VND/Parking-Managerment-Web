export class CarReportDto {
  id!: number | null;
  customerName!: string;
  userId!: number;
  licensePlate!: string;
  reason!: string;
  content!: string;
  creationTime!: Date;
}
