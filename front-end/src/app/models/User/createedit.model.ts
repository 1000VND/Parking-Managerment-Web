export class CreateEditUserDto {
    id: number | undefined | null;
    userName: string = '';
    passWord: string = '';
    fullName: string = '';
    role: number | undefined;
}