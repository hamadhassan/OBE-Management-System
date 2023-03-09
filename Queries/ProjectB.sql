
SELECT C.Name FROM CLO C
SELECT C.Id FROM Clo C WHERE C.Name=

SELECT * FROM Assessment A

SELECT AC.Id FROM AssessmentComponent AC WHERE AC.Name=
SELECT A.Id FROM Assessment A WHERE A.Title=
INSERT INTO StudentResult VALUES (32,2,1,GETDATE())


SELECT * FROM Student

SELECT * FROM RubricLevel

SELECT * FROM Assessment

SELECT A.Id,A.Title,A.DateCreated,A.TotalMarks,A.TotalWeightage FROM Assessment A

SELECT * FROM StudentResult









SELECT * FROM AssessmentComponent
DECLARE @entered_marks INT = 1;
DECLARE @AssessmentId INT = 1;
DECLARE @existing_marks INT = (SELECT SUM(AC.TotalMarks) FROM Assessment A JOIN AssessmentComponent AC ON A.Id=AC.AssessmentId WHERE A.Id=@AssessmentId )

DECLARE @SubUpdateMarks INT=(SELECT AC.TotalMarks FROM AssessmentComponent AC WHERE AC.Id=2)
DECLARE @total_marks INT = (SELECT A.TotalMarks FROM Assessment A  WHERE A.Id=@AssessmentId)
SELECT @total_marks

IF (@existing_marks + @entered_marks <= @total_marks)
    SELECT 'True';
ELSE
    SELECT 'False';

SELECT * FROM LOOKUP


--------------------------------------------------------------------------------
SELECT  DISTINCT TOP 5 CONCAT(S.FirstName,' ',S.LastName)AS [Student Name],S.RegistrationNumber
FROM Student S
JOIN StudentAttendance SA
ON SA.StudentId=S.Id
JOIN ClassAttendance CA
ON CA.Id=SA.AttendanceId
JOIN Lookup L
ON L.LookupId=SA.AttendanceStatus
WHERE L.Name='Present'
ORDER BY S.RegistrationNumber DESC 

SELECT A.Title AS Name, A.TotalMarks,A.TotalWeightage FROM Assessment A

SELECT C.Name as [CLO Name],R.Details AS [Rubric Name] FROM Clo C JOIN Rubric R ON R.CloId=c.Id
-----------------------------------STUDENT RESULT--------------------------------------------------------
DECLARE @MaxLevel as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId IN (SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id))
DECLARE @AllComponentTotalMarks as float=(SELECT SUM(A.TotalMarks) FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id JOIN Assessment A ON A.Id=AC.AssessmentId)
SELECT  CONCAT(S.FirstName,' ',S.LastName) AS [Student Name],A.Title AS [Assessmmet Name],
(RL.MeasurementLevel/@MaxLevel)*AC.TotalMarks AS[Obtained Marks],@AllComponentTotalMarks AS [Total Marks]
FROM StudentResult SR
JOIN Student S
ON S.Id=SR.StudentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN Rubric R
ON R.Id=RL.RubricId
JOIN AssessmentComponent AC
ON AC.ID=SR.AssessmentComponentId
JOIN Assessment A
ON A.Id=AC.AssessmentId
ORDER BY [Obtained Marks] DESC



-----------------------------------------------------
SELECT COUNT(ID) FROM Assessment
SELECT COUNT(AC.Id) FROM AssessmentComponent AC
SELECT COUNT(R.Id) FROM Rubric R
SELECT COUNT(RL.Id) FROM RubricLevel RL
SELECT COUNT(C.Id) FROM Clo C
SELECT COUNT(CA.Id) FROM ClassAttendance CA
------------------------------------------------------------------------------------------
SELECT S.id,S.FirstName,S.lastname,S.Contact,S.email,S.RegistrationNumber
FROM StudentResult SR
JOIN Student S
ON SR.StudentId=S.Id
JOIN Lookup L
ON L.LookupId=S.Status
WHERE L.Category='STUDENT_STATUS'

SELECT * FROM Student

SELECT S.id,S.FirstName,S.lastname,S.Contact,S.email,S.RegistrationNumber,L.Name AS Status FROM Student S JOIN Lookup L ON L.LookupId=S.Status WHERE  L.Category='STUDENT_STATUS'

SELECT *
FROM Lookup

SELECT *  FROM AssessmentComponent AC WHERE AC.RubricId=1 AND AC.Name='Question1'

SELECT *
FROM StudentResult SR
JOIN AssessmentComponent AC
ON SR.AssessmentComponentId=AC.Id
JOIN RubricLevel RL
ON RL.RubricId=AC.RubricId
WHERE SR.RubricMeasurementId=RL.Id


SELECT * 
FROM StudentAttendance SA

SELECT Name FROM Lookup WHERE Category='ATTENDANCE_STATUS'


SELECT S.Id,CONCAT(S.FirstName,' ',S.LastName) AS [Student Name],S.RegistrationNumber 
FROM Student S 
JOIN Lookup L
ON L.LookupId=S.Status
WHERE L.Category='STUDENT_STATUS' AND L.Name='Active'



SELECT * from Lookup


---------The Below statments for student id=6---------------------------
DECLARE @StudentId AS int =6
DECLARE @TotalMarks as float =(SELECT AC.TotalMarks FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id WHERE SR.StudentId=@StudentId)
SELECT @TotalMarks
DECLARE @RubricId1 as float=(SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id WHERE SR.StudentId=@StudentId)
DECLARE @MaxLevel1 as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId=@RubricId1)
DECLARE @ObtainedLevel as float=(SELECT RL.MeasurementLevel FROM StudentResult SR JOIN RubricLevel RL ON SR.RubricMeasurementId=RL.Id WHERE SR.StudentId=@StudentId)
SELECT CONCAT(S.FirstName,' ',S.LastName) ,(@ObtainedLevel/@MaxLevel1) *@TotalMarks
FROM StudentResult SR
JOIN Student S
ON S.Id=SR.StudentId
WHERE SR.StudentId=@StudentId

--Display Student Name,Max Level,Obtained Level, Total Marks, Obtained Marks, Assessment Name
DECLARE @MaxLevel as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId IN (SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id))
SELECT  CONCAT(S.FirstName,' ',S.LastName) AS [Full Name],@MaxLevel AS [Maximum Level] ,RL.MeasurementLevel,AC.TotalMarks, (RL.MeasurementLevel/@MaxLevel)*AC.TotalMarks AS[Obtained Marks],
AC.Name AS [Question Name],A.Title AS [Assessmmet Name],SR.AssessmentComponentId,SR.StudentId
FROM StudentResult SR
JOIN Student S
ON S.Id=SR.StudentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN Rubric R
ON R.Id=RL.RubricId
JOIN AssessmentComponent AC
ON AC.ID=SR.AssessmentComponentId
JOIN Assessment A
ON A.Id=AC.AssessmentId


SELECT SR.StudentId,SR.AssessmentComponentId,SR.RubricMeasurementId,SR.EvaluationDate FROM StudentResult SR
SELECT TOP 1 CA.Id FROM ClassAttendance CA ORDER BY CA.ID DESC

SELECT CONCAT(S.FirstName,' ',S.LastName),S.RegistrationNumber FROM Student S WHERE S.Id=6
SELECT L.Name FROM Lookup L WHERE L.Category='ATTENDANCE_STATUS'
DELETE FROM StudentResult WHERE StudentId=24 AND AssessmentComponentId=5
SELECT S.Id FROM Student S  JOIN Lookup L ON L.LookupId=S.Status  WHERE L.Category='STUDENT_STATUS' AND L.Name='Active'
SELECT * FROM StudentAttendance
SELECT * FROM ClassAttendance
------------------------Attendance------------------------------------------------
INSERT INTO ClassAttendance(AttendanceDate) VALUES(GETDATE())
DECLARE @AttendanceStatus  INT = 1
DECLARE @ClassAttendanceID  INT = (SELECT TOP 1 CA.Id FROM ClassAttendance CA ORDER BY CA.ID DESC)
DECLARE @Counter  BIGINT = 0
DECLARE @RowCount BIGINT = 0
SET @Counter=0
SELECT @RowCount =Count(S.Id) FROM Student S  JOIN Lookup L ON L.LookupId=S.Status  WHERE L.Category='STUDENT_STATUS' AND L.Name='Active'
WHILE ( @Counter < @RowCount)
BEGIN
	DECLARE @idStudent as INT =(SELECT S.Id FROM Student S  JOIN Lookup L ON L.LookupId=S.Status  WHERE L.Category='STUDENT_STATUS' AND L.Name='Active' 
	ORDER BY S.Id OFFSET @Counter ROWS   FETCH NEXT 1 ROWS ONLY)
	INSERT INTO StudentAttendance(AttendanceId,StudentId,AttendanceStatus) VALUES(@ClassAttendanceID,@idStudent,@AttendanceStatus)
    SET @Counter  = @Counter  + 1
END

------------------------------------------------------------------------
SELECT CONCAT(S.FirstName,' ',S.LastName)AS [Student Name],S.RegistrationNumber,L.Name,S.Id,SA.AttendanceId
FROM Student S
JOIN StudentAttendance SA
ON S.Id=SA.StudentId
JOIN ClassAttendance CA
ON CA.Id=SA.AttendanceId
JOIN Lookup L
ON L.LookupId=SA.AttendanceStatus
WHERE CA.AttendanceDate='2023-03-06'
ORDER BY RegistrationNumber

---------------------------------------------------------------------------

--STUDENTS
--CRUD Operation
SELECT *
FROM Student
INSERT INTO STUDENT(FirstName,LastName,Contact,Email,RegistrationNumber,Status)
VALUES('Syed Hashir','Husnain','03000000000','abc@example.com','2021-CS-1',1)
INSERT INTO STUDENT(FirstName,LastName,Contact,Email,RegistrationNumber,Status)
VALUES('Kabir','Ali','03000000000','abc@example.com','2021-CS-4',5)
INSERT INTO STUDENT(FirstName,LastName,Contact,Email,RegistrationNumber,Status)
VALUES('Hammad','Hassan','03000000000','abc@example.com','2021-CS-33',1)
INSERT INTO STUDENT(FirstName,LastName,Contact,Email,RegistrationNumber,Status)
VALUES('ABC','ABC','03000000000','abc@example.com','2021-CS-250',2)
DELETE from STUDENT WHERE ID=2
UPDATE Student SET Contact='03000000001' WHERE RegistrationNumber='2021-CS-3'
--STUDENT ATTENDANCE
SELECT *
FROM StudentAttendance
INSERT INTO StudentAttendance(AttendanceId,StudentId,AttendanceStatus) VALUES(5,6,1)
DELETE FROM StudentAttendance WHERE StudentId=2
UPDATE StudentAttendance SET AttendanceStatus=0 WHERE  AttendanceId=5 AND StudentId=6 
-- CLASS ATTRNDANCE (Class attendace ka bagar student attendance nahi lag sakti)
SELECT * 
FROM ClassAttendance
INSERT INTO ClassAttendance(AttendanceDate) VALUES('2023-03-05')
DELETE ClassAttendance WHERE ID=2
UPDATE ClassAttendance SET AttendanceDate='2023-03-06' where ID=20
-- CLO 
SELECT *
FROM Clo
INSERT INTO Clo VALUES('CLO 2: Implement abstraction',GETDATE(),GETDATE())
UPDATE CLO SET Name ='CLO 1: Apply composition, inheritance' WHERE Id=1
--RUBRIC
SELECT *
FROM Rubric
INSERT INTO Rubric(Id,Details,CloId) VALUES(4,'Testing',2)
UPDATE Rubric SET Details='Design ' WHERE ID=1
--RUBRIC LEVEL
SELECT * 
FROM RubricLevel
INSERT INTO RubricLevel VALUES(2,
'Program does not compile or interpret due to lack of syntax knowledge'
,1)
UPDATE RubricLevel SET RubricId=1 WHERE ID=1
--Assessment 
SELECT *
FROM Assessment
INSERT INTO Assessment VALUES('Quiz123',GETDATE(),10,10)
--Assessment Components
SELECT *
FROM AssessmentComponent
INSERT INTO AssessmentComponent VALUES('Question-3',1,5,GETDATE(),GETDATE(),1)
-- Student Result
SELECT *
FROM StudentResult
INSERT INTO StudentResult VALUES(9,3,5,GETDATE())
-----------------------------------------------------------------
-----------------------------------------------------------------
--1a. Give students name present in 2023
SELECT CONCAT(S.FirstName,' ',S.LastName) [FULL NAME]
FROM Student S JOIN StudentAttendance SA 
ON S.Id=SA.StudentId
JOIN ClassAttendance CA
ON CA.Id=SA.AttendanceId
WHERE YEAR(CA.AttendanceDate)='2023' AND month(CA.AttendanceDate)='2'
--1b. Give the student list who absent in 2023
SELECT CONCAT(S.FirstName,' ',S.LastName) [FULL NAME]
FROM Student S JOIN StudentAttendance SA 
ON S.Id=SA.StudentId
JOIN ClassAttendance CA
ON CA.Id=SA.AttendanceId
WHERE SA.AttendanceStatus is null 
--2a Give CLO-2 Rubric Detail and mearsument level number
SELECT RL.Details,Rl.MeasurementLevel
FROM Rubric R JOIN  RubricLevel RL
ON R.Id=RL.RubricId
WHERE R.CloId=2
--2b Give Rubric-1 all Rubrics Level Detail
SELECT RL.Details,Rl.MeasurementLevel
FROM Rubric R JOIN  RubricLevel RL
ON R.Id=RL.RubricId
WHERE R.Id=1;
--3 Give Assessment-1 all Components name and total marks
SELECT AC.Name,AC.TotalMarks
FROM Assessment A JOIN AssessmentComponent AC
ON A.Id=AC.AssessmentId
WHERE A.Id=1
-- 4 Give Assessment-1 corressponding CLO Number 
SELECT R.CloId
FROM Assessment A JOIN AssessmentComponent AC
ON A.Id=AC.AssessmentId
JOIN Rubric R
ON AC.RubricId=R.Id
WHERE CloId=2
--5 Give the Assessment-1 Rubric
SELECT AC.Name,R.Details,AC.TotalMarks
FROM AssessmentComponent AC JOIN Rubric R
ON AC.RubricId=R.Id
WHERE AC.AssessmentId=1
-- 6 Get the Student Name who assessment is added into the system
SELECT CONCAT(S.FirstName,' ',S.LastName) [Full Name]
FROM Student S JOIN StudentResult SR
ON S.Id=SR.StudentId
WHERE SR.StudentId IS NOT NULL
-- 7 Calculate the student Obtained marks in the assessemnt 
SELECT AC.Name AS Component,R.Details AS Rubric,AC.TotalMarks AS [Component Marks]
,(SELECT Max(RL.MeasurementLevel) AS [Student Level Rubric]
FROM StudentResult JOIN RubricLevel RL 
ON SR.RubricMeasurementId=RL.Id)
,(SELECT Max((3/4)*10)
FROM StudentResult SR JOIN RubricLevel RL 
ON SR.RubricMeasurementId=RL.Id 
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId) 
FROM StudentResult SR JOIN AssessmentComponent AC  
ON  SR.AssessmentComponentId=AC.Id
JOIN Rubric R
ON R.Id=AC.RubricId

--8 Count the students who fullfill the particular CLO
SELECT CONCAT(S.FirstName,' ',S.LastName) [Full Name]
FROM StudentResult SR JOIN RubricLevel  RL
ON SR.RubricMeasurementId=RL.Id
JOIN Student S
ON  S.Id=SR.StudentId
JOIN Rubric R 
ON R.Id=RL.RubricId
JOIN Clo C
ON C.Id=R.CloId
WHERE CloId=2
--9 Find the dublicate who attened the same CLO
SELECT CONCAT(S.FirstName,' ',S.LastName) [Full Name]
FROM StudentResult SR JOIN RubricLevel  RL
ON SR.RubricMeasurementId=RL.Id
JOIN Student S
ON  S.Id=SR.StudentId
JOIN Rubric R 
ON R.Id=RL.RubricId
JOIN Clo C
ON C.Id=R.CloId
WHERE CloId=2
GROUP BY S.FirstName,S.LastName
HAVING COUNT(S.FirstName)>1
-- 10 Display all enrolled students
SELECT *
FROM Student S
WHERE S.Status=1
-- 11 Display all the assignemnt with total weightage
SELECT SUM(A.TotalWeightage) AS [Total Weightages]
FROM Assessment A





