-----------------------------------TOP 10 STUDENT--------------------------------------------------------------------------------

SELECT TOP 10 CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks]
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks
ORDER BY [Obtained Marks] DESC

------------------------------------------------------------------
-----------------------------------CLO WISE RESULT--------------------------------------------------------------------------------

SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks],
C.Name AS [CLOName]
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
JOIN Rubric R
ON R.Id=AC.RubricId
JOIN Clo C
ON C.Id=R.CloId
WHERE C.Id=2
GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks,C.Name
ORDER BY C.Name ASC
-------------------------------8----CLO Fail--------------------------------------------------------------------------------

SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks],
C.Name AS [CLOName]
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
JOIN Rubric R
ON R.Id=AC.RubricId
JOIN Clo C
ON C.Id=R.CloId
GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks,C.Name
HAVING  ((SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks))/A.TotalMarks)*100<=33
ORDER BY C.Name ASC

-------------------------------------------------------
-----------------------------------Assessment WISE RESULT--------------------------------------------------------------------------------

SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks]
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
JOIN Rubric R
ON R.Id=AC.RubricId
JOIN Clo C
ON C.Id=R.CloId
WHERE A.Id=2
GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks
ORDER BY  A.Title ASC
-----------------------------------Assessment Component WISE RESULT--------------------------------------------------------------------------------

SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,AC.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks]
,AC.Name AS QuestionName
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
JOIN Rubric R
ON R.Id=AC.RubricId
JOIN Clo C
ON C.Id=R.CloId
WHERE AC.Id=2
GROUP BY S.RegistrationNumber,A.Title,AC.TotalMarks,AC.Name
ORDER BY  AC.Name ASC
----------------------------------Indiduale Result---------------------------------
SELECT CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],A.Title AS Assignment,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks],A.TotalMarks
FROM Student S
JOIN StudentResult SR
ON SR.StudentId=S.Id
JOIN AssessmentComponent AC
ON AC.Id=SR.AssessmentComponentId
JOIN RubricLevel RL
ON RL.Id=SR.RubricMeasurementId
JOIN  Assessment A
ON A.Id=AC.AssessmentId
JOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML
ON ML.RubricId=AC.RubricId
WHERE S.Id=24
GROUP BY S.RegistrationNumber,A.Title,A.TotalMarks
ORDER BY S.RegistrationNumber ASC
--------------------------------------------------Student Attendance--------------------------------------
SELECT  CONVERT(date,  C.AttendanceDate, 101) AS Date ,L.Name
FROM Student S
JOIN StudentAttendance ST
ON S.Id=ST.StudentId
JOIN ClassAttendance C
ON C.Id=ST.AttendanceId
JOIN Lookup L
ON L.LookupId=ST.AttendanceStatus
WHERE S.Id=29
ORDER BY DATE DESC
-----------------------Class Attendance----------------------

SELECT * FROM ClassAttendance

Declare @StudentId As int=1
--Total Attendance
Declare @TotalClassAttendance As int =(SELECT COUNT(CT.Id)
FROM ClassAttendance CT)
--PresentStudentCount
Declare @PresentCount As int=(SELECT COUNT(S.Id)AS PresentStudentCount
FROM Student S
JOIN StudentAttendance ST
ON S.Id=ST.StudentId
JOIN ClassAttendance C
ON C.Id=ST.AttendanceId
JOIN Lookup L
ON L.LookupId=ST.AttendanceStatus
WHERE L.Category='ATTENDANCE_STATUS' AND L.Name='Present'AND ST.StudentId=@StudentId)
--AbsentStudentCount
DECLARE @AbsentCount as float=(SELECT COUNT(S.Id)AS AbsentStudentCount
FROM Student S
JOIN StudentAttendance ST
ON S.Id=ST.StudentId
JOIN ClassAttendance C
ON C.Id=ST.AttendanceId
JOIN Lookup L
ON L.LookupId=ST.AttendanceStatus
WHERE L.Category='ATTENDANCE_STATUS' AND L.Name<>'Present' AND ST.StudentId=@StudentId)

SELECT @TotalClassAttendance,@AbsentCount,@PresentCount
------------------------------------Class Attendnce==================================
SELECT CONCAT(S.FirstName,' ',S.LastName)AS Name,S.RegistrationNumber,L.Name
FROM Student S
JOIN StudentAttendance ST
ON S.Id=ST.StudentId
JOIN ClassAttendance C
ON C.Id=ST.AttendanceId
JOIN Lookup L
ON L.LookupId=ST.AttendanceStatus
WHERE L.Category='ATTENDANCE_STATUS' AND C.AttendanceDate='2023-03-10 00:00:00.000'
