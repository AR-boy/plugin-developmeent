
import sys
import cv2
from cv2 import aruco

if __name__ == '__main__':
    if len(sys.argv) - 1 != 4 :
        print("incorrect number of arguments - arguments necessary are: number of squaresW, squaresH, squareLength and markerLength")
    else :
        
        numOfSquaresW = sys.argv[1]
        numOfSquaresH = sys.argv[2]
        squareLength = sys.argv[3]
        markerLength = sys.argv[4]

        charuco_dict =  aruco.Dictionary_get(aruco.DICT_4X4_50)
        charuco_board = aruco.CharucoBoard_create(int(numOfSquaresW), int(numOfSquaresH), float(squareLength), float(markerLength), charuco_dict)
        charuco_board_image = charuco_board.draw((2000, 2000))
        cv2.imwrite(("CharucoBoard_"+numOfSquaresW+"_"+numOfSquaresH+"_"+squareLength+"_"+markerLength+".png"), charuco_board_image)
        print("generated CharucoBoard image")