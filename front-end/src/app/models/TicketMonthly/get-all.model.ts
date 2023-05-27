export class GetAllDataTicketMonthlyDto{
  id!: number;
  creationTime!: Date;
  lastModificationTime!: Date;
  isDelete!: boolean;

  licensePlate!: string;
  customerImage!: string;
  customerName!: string;
  phoneNumber!: string;
  customerAddress!: string;
  birthday!: Date;
  gender!: boolean;
  lastRegisterDate!: Date;
  customerPoint!: number;
}
